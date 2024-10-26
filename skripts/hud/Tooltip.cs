using System;
using Godot;

[GlobalClass]
public partial class Tooltip : PanelContainer
{
    private TooltipPanel tooltipPanel;

    [Export(PropertyHint.Range, "0,100,1")]
    public float OffsetX;
    [Export(PropertyHint.Range, "0,100,1")]
    public float OffsetY;
    [Export(PropertyHint.Range, "0,100,1")]
    public float PaddingX;
    [Export(PropertyHint.Range, "0,100,1")]
    public float PaddingY;

    private Control parent;
    private Control topParent;

    private Vector2 offset;
    private Vector2 padding;
    private string TooltipCache;

    public override void _Ready()
    {
        offset = new Vector2(OffsetX, OffsetY);
        padding = new Vector2(PaddingX, PaddingY);

        parent = GetParent<Control>();
        topParent = parent;

        while (topParent.GetParent() is Control c)
        {
            topParent = c;
        }

        parent.MouseEntered += MyOnMouseEntered;
        parent.MouseExited += MyOnMouseExited;
    }

    public override void _Process(double delta)
    {
        if (tooltipPanel != null && tooltipPanel.Visible)
        {
            var border = topParent.Size - padding;
            var extents = tooltipPanel.Size;
            var base_pos = parent.GetScreenPosition();

            // test if need to display to the left
            var final_x = base_pos.X + offset.X;
            if (final_x + extents.X > border.X)
            {
                final_x = Math.Max(0, base_pos.X - offset.X - extents.X);
            }

            // test if need to display below
            var final_y = base_pos.Y - extents.Y - offset.Y;
            if (final_y < padding.Y)
            {
                final_y = Math.Max(0, base_pos.Y + offset.Y);
            }

            tooltipPanel.Position = new Vector2(final_x, final_y);
        }
    }

    private void MyOnMouseEntered()
    {
        tooltipPanel = ResourceLoader.Load<PackedScene>("res://scenes/hud/TooltipPanel.tscn").Instantiate<TooltipPanel>();
        tooltipPanel.Visible = false;

        topParent.CallDeferred(MethodName.AddChild, tooltipPanel);
        TooltipCache = parent.TooltipText;
        parent.TooltipText = null;
        tooltipPanel.Visible = true;
        tooltipPanel.SetMyTooltipText(TooltipCache);
    }

    private void MyOnMouseExited()
    {
        parent.TooltipText = TooltipCache;
        if (tooltipPanel != null)
        {
            tooltipPanel.QueueFree();
            tooltipPanel = null;
        }
    }
}
