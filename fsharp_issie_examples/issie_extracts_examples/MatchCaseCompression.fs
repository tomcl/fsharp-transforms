// --------------------------------  Match Case Compression -------------------------------- //

// Match case compression means moving common code outside match expressions to reduce duplication.
// Move common code inside match case expressions, outside, before the match.
// Note that this may require the match case expressions (or part of them)
// to be turned into functions so that the “variable” bits can be factored out.
// But often all that is needed is let definitions before.

module MatchCaseCompression

open Elmish
open CommonTypes
open DrawModelType
open DrawModelType.SheetT
open DrawModelType.SymbolT

// setups for the extracts 
// this is also examples of global helper functions - see GlobalHelperFunction.fs
let symbolCmd (msg: SymbolT.Msg) = Cmd.ofMsg (ModelType.Msg.Sheet (Wire (BusWireT.Symbol msg)))
let wireCmd (msg: BusWireT.Msg) = Cmd.ofMsg (ModelType.Msg.Sheet (Wire msg))
let sheetCmd (msg: SheetT.Msg) = Cmd.ofMsg (ModelType.Msg.Sheet msg)

let beforeCleanUpVer model compId =
    let button = model.Wire.Symbol.Symbols[compId]
    match button.STransform.Rotation with
    | Degree90 ->
        {model with TmpModel = Some model}, 
            Cmd.batch [ sheetCmd (Rotating Clockwise); 
                        wireCmd (BusWireT.UpdateConnectedWires model.SelectedComponents)]
    | _ -> // can do this since Degree0 and Degree180 is not a case for those Buttons
        {model with TmpModel = Some model}, 
            Cmd.batch [ sheetCmd (Rotating AntiClockwise); 
                        wireCmd (BusWireT.UpdateConnectedWires model.SelectedComponents)]

// cleaner version
// optimisation = function-after-match
let extracted model compId =
    let button = model.Wire.Symbol.Symbols[compId]
    match button.STransform.Rotation with
    | Degree90 -> Clockwise
    | _ -> AntiClockwise
    |> (fun rot ->
            {model with TmpModel = Some model; Action = Idle}, 
                Cmd.batch [ sheetCmd (Rotating rot); 
                            wireCmd (BusWireT.UpdateConnectedWires model.SelectedComponents)])