namespace Hathor

open Hathor.FSharp

type A() =
    static member internal_system id name description labels = SoftwareSystem.create id name description (labels |> Set.ofList) Location.Internal
    
    static member user id name description tags location = User.create id name description (tags |> List.map Tag |> Tag.ofList) location
    
    static member internal_person id name description = 
        User.create id name description (Tag.single Tag.Person) Location.Internal
        |> User.addTag Tag.Person

    static member external_person id name description = 
        User.create id name description (Tag.single Tag.Person) Location.Internal
        |> User.addTag Tag.Person