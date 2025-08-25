module SeaottermsSiteFileserver.Models.Model
// ---------------------------------
// Request Models(unuse)
// ---------------------------------

type Message =
    {
        Text : string
    }

// ---------------------------------
// DB Models
// ---------------------------------

type User() =
    [<DefaultValue>] val mutable Id : int
    [<DefaultValue>] val mutable Name : string