namespace Hathor.Structurizr
[<AutoOpen>]
module DSL =
    open System
    open System.IO

    type SystemLandscapeDiagramBuilder internal (scope, desc, size) =
        
        member __.Yield(_) : SystemLandscapeDiagram = 
            SystemLandscapeDiagram.init scope desc size

        [<CustomOperation("person")>]
        member __.Person(diagram, (user:Hathor.User)) : SystemLandscapeDiagram =
            let u = user |> SystemViewElement.ofPerson
            diagram |> SystemLandscapeDiagram.addElement u
            //match u with
            //| UserType.DesktopApp _ -> diagram |> SystemLandscapeDiagram.addElement (SystemViewElement.User u)
            //| UserType.Mobile _ -> diagram |> SystemLandscapeDiagram.addElement (SystemViewElement.User u)
            //| UserType.Person _ -> diagram |> SystemLandscapeDiagram.addPerson (u)
            //| UserType.WebBrowser _ -> diagram |> SystemLandscapeDiagram.addElement (SystemViewElement.User u)
            
        [<CustomOperation("system")>]
        member __.System(diagram, (system:Hathor.SoftwareSystem)) : SystemLandscapeDiagram =
            let s = system |> SoftwareSystemElement.ofSystem
            diagram |> SystemLandscapeDiagram.addSoftwareSystem s

        //[<CustomOperation("relationship")>]
        //member __.Relationship(diagram, source, description, destination) : SystemLandscapeDiagram =
        //    let srcElOpt = diagram |> SystemLandscapeDiagram.findElementByName source
        //    let destElOpt = diagram |> SystemLandscapeDiagram.findElementByName destination
        //    match srcElOpt,destElOpt with
        //    | _,None -> diagram
        //    | None,_ -> diagram
        //    | Some srcEl, Some destEl ->
        //        let relationship = Relationship.between (srcEl |> Element.ofSystemViewElement) description (destEl |> Element.ofSystemViewElement)
        //        diagram |> SystemLandscapeDiagram.addRelationship relationship        

    let system_landscape_diagram scope desc size = SystemLandscapeDiagramBuilder(scope,desc,size)

    let save (path:string) json =
        let folder = Path.GetDirectoryName(path)
        if(String.IsNullOrWhiteSpace(folder) |> not && Directory.Exists(folder) |> not) then 
            Directory.CreateDirectory(folder) |> ignore
        File.WriteAllText(path, json) |> ignore