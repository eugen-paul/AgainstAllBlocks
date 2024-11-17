extends Node

func FUNC_GET_X_POINTS(n: int) -> String:
    return tr_n("GET_X_POINTS.", "GET_X_POINTS_PLURAL.", n)

func FUNC_CATCH_X_ROCKETS(n: int) -> String:
    return tr_n("CATCH_X_ROCKETS.","CATCH_X_ROCKETS_PLURAL.",n)
    
func FUNC_CATCH_X_BOMBS(n: int) -> String:
    return tr_n("CATCH_X_BOMBS.","CATCH_X_BOMBS_PLURAL.",n)
    
var DONT_USE_ROCKETS:String = tr("DONT_USE_ROCKETS.")
var DONT_LOSE_ANY_LIFE:String = tr("DONT_LOSE_ANY_LIFE.")
var DONT_CATCH_ANY_ITEMS:String = tr("DONT_CATCH_ANY_ITEMS.")
