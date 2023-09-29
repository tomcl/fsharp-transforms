// -------------------------------- Match lists -------------------------------- //

// The great advantage of match on lists is that the first one or several elements can be extracted as separate identifiers,
// and if this is not possible a suitable alternative thing can be done. 
// So match combines check for list length and list indexing in away that obeys NEP (no exceptions possible). (NEP, RSN).

module MatchLists

open CommonTypes
open SimulatorTypes

let rec checkComponentPorts (ports: Port list) (correctType: PortType) =
    match ports with
    | [] -> None
    | port :: _ when port.PortNumber = None -> // match lists, also using guards
        Some
            { ErrType = PortNumMissing correctType
              InDependency = None
              ComponentsAffected = [ ComponentId port.HostId ]
              ConnectionsAffected = [] }
    | port :: _ when port.PortType <> correctType -> // match lists, also using guards
        Some 
            { ErrType = WrongPortType (correctType, port)
              InDependency = None
              ComponentsAffected = [ ComponentId port.HostId ]
              ConnectionsAffected = [] }
    | _ :: ports' -> checkComponentPorts ports' correctType // match lists