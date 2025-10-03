extends Node

func FUNC_GET_X_POINTS(n: int) -> String:
    return tr_n("GET_X_POINTS.", "GET_X_POINTS_PLURAL.", n)

func FUNC_CATCH_X_ROCKETS(n: int) -> String:
    return tr_n("CATCH_X_ROCKETS.","CATCH_X_ROCKETS_PLURAL.",n)

func FUNC_CATCH_X_BOMBS(n: int) -> String:
    return tr_n("CATCH_X_BOMBS.","CATCH_X_BOMBS_PLURAL.",n)

func FUNC_LEVEL_15_X_GOALS_K_SECONDS(goals: int, seconds: int) -> String:
    var g = tr_n("GOAL","GOALS",goals, "Level 15 Achivment")
    var s = tr_n("SECOND","SECONDS",seconds, "Level 15 Achivment")
    return tr("{goals} {g} in {seconds} {s}").format(
        {
            goals = goals, 
            g = g,
            seconds = seconds,
            s = s
        })

func FUNC_GET_X_ACTIVE_BALLS(n: int) -> String:
    return tr_n("GET_X_ACTIVE_BALLS.","GET_X_ACTIVE_BALLS_PLURAL.",n)

var DONT_USE_ROCKETS:String = tr("DONT_USE_ROCKETS.")
var DONT_LOSE_ANY_LIFE:String = tr("DONT_LOSE_ANY_LIFE.")
var DONT_CATCH_ANY_ITEMS:String = tr("DONT_CATCH_ANY_ITEMS.")
