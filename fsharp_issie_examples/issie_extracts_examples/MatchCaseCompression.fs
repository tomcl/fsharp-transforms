// -------------------------------- Adding Parameters -------------------------------- //

// Where two blocks of code can't immediately be commoned up as a value or function used twice,
// adding a parameter can make this possible. Choose function and parameter name and operation so that what it does is clear.

module AddingParameters


open Elmish
open CommonTypes
open DrawModelType
open DrawModelType.SheetT
open DrawModelType.SymbolT

// set ups for the extracts to compile
let symbolCmd (msg: SymbolT.Msg) = Cmd.ofMsg (ModelType.Msg.Sheet (Wire (BusWireT.Symbol msg)))
let wireCmd (msg: BusWireT.Msg) = Cmd.ofMsg (ModelType.Msg.Sheet (Wire msg))
let sheetCmd (msg: SheetT.Msg) = Cmd.ofMsg (ModelType.Msg.Sheet msg)


let beforeCleanUpVer model compId =
    let button = model.Wire.Symbol.Symbols[compId]
    match button.STransform.Rotation with
    | Degree90 ->
        {model with TmpModel = Some model}, 
            Cmd.batch [ sheetCmd (Rotate RotateClockwise); 
                        wireCmd (BusWireT.UpdateConnectedWires model.SelectedComponents)]
    | _ -> // can do this since Degree0 and Degree180 is not a case for those Buttons
        {model with TmpModel = Some model}, 
            Cmd.batch [ sheetCmd (Rotate RotateAntiClockwise); 
                        wireCmd (BusWireT.UpdateConnectedWires model.SelectedComponents)]

// cleaner version extracted from Issie
// optimisation = function-after-match
let extracted model compId =
    let button = model.Wire.Symbol.Symbols[compId]
    match Button.STransform.Rotation with
    | Degree90 -> RotateClockwise
    | _ -> RotateAntiClockwise
    |> (fun rot ->
            {model with TmpModel = Some model; Action = Idle}, 
                Cmd.batch [ sheetCmd (Rotate rot); 
                            wireCmd (BusWireT.UpdateConnectedWires model.SelectedComponents)])