extends Node

func FUNC_GET_X_POINTS(n: int) -> String:
    return tr_n("Get at least {0} point.", "Get at least {0} points.", n)

func FUNC_CATCH_X_ROCKETS(n: int) -> String:
    return tr_n("Catch at least {0} rocket.","Catch at least {0} rockets.",n)
    
func FUNC_CATCH_X_BOMBS(n: int) -> String:
    return tr_n("Catch at least {0} bomb.","Catch at least {0} bombs.",n)
    
var DONT_USE_ROCKETS:String = tr("Do not use rockets.")
var DONT_LOSE_ANY_LIFE:String = tr("Don't lose any life.")
var DONT_CATCH_ANY_ITEMS:String = tr("Don't catch any items.")
