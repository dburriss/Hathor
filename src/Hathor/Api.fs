namespace Hathor

open Hathor.FSharp

type A() =
    static member Person id name description location = Person.create id name description location
    static member System id name description labels location = SoftwareSystem.create id name description labels location