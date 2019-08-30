namespace Hathor

type Location = Internal | External

type Tag = private Tag of string

type Relationship = {
    Description : string
    Technology : string Set
    Between : string * string
    Tags : Tag Set
}
    
type User = {
    Id : string
    Name : string
    Description : string
    Location : Location
    Relationships : Relationship Set
    Tags : Tag Set
}

type CodeElement = {
    Id : string
    Name : string
    Description : string
    FullTypeName: string
    Relationships : Relationship Set
    Tags : Tag Set
}

type Component = {
    Id : string
    Name : string
    Description : string
    Technology : string Set
    CodeElements : CodeElement Set
    Relationships : Relationship Set
    Tags : Tag Set
}

type Container = {
    Id : string
    Name : string
    Description : string
    Technology : string Set
    Components : Component list
    Relationships : Relationship Set
    Tags : Tag Set
}

type SoftwareSystem = {
    Id : string
    Name : string
    Description : string
    Labels : string Set
    Location : Location
    Containers : Container list
    Relationships : Relationship Set
    Tags : Tag Set
}

type Landscape = {
    Id : string
    Name : string
    Description : string
    Labels : string Set
    Systems : SoftwareSystem list
    Relationships : Relationship Set
    Tags : Tag Set
}
