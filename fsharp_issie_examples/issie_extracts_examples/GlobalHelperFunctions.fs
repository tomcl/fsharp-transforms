// -------------------------------- Global Helper Functions -------------------------------- //

module GlobalHelperFunctions

open Elmish
open CommonTypes
open DrawModelType
open DrawHelpers
open DrawModelType.SymbolT
open DrawModelType.SheetT
open DrawModelType.BusWireT
open Optics
open FSharp.Core

// global helper functions
// look up how many times below are used in various functions in issie
// the extracted code is just a section of it that uses these
let symbolCmd (msg: SymbolT.Msg) = Cmd.ofMsg (ModelType.Msg.Sheet (Wire (BusWireT.Symbol msg)))
let wireCmd (msg: BusWireT.Msg) = Cmd.ofMsg (ModelType.Msg.Sheet (Wire msg))
let sheetCmd (msg: SheetT.Msg) = Cmd.ofMsg (ModelType.Msg.Sheet msg)

let extracted1Update (msg : SheetT.Msg) (issieModel : ModelType.Model): ModelType.Model*Cmd<ModelType.Msg> =
    let model = issieModel.Sheet
    match msg with
    | KeyPress CtrlC ->
        model,
        Cmd.batch [
            symbolCmd (CopySymbols model.SelectedComponents) // global helper function
            wireCmd (CopyWires model.SelectedWires) // global helper function
        ]

    | KeyPress ESC -> 
        match model.Action with
        | DragAndDrop ->
            { model with SelectedComponents = []
                         SelectedWires = []
                         Action = Idle },
            Cmd.batch [ symbolCmd (DeleteSymbols (model.SelectedComponents)) // global helper function
                        wireCmd (DeleteWires model.SelectedWires) // global helper function
                        sheetCmd UpdateBoundingBoxes // global helper function
                      ]
        | _ -> model, Cmd.none

    | KeyPress CtrlZ ->
        match model.UndoList with
        | [] -> model , Cmd.none
        | prevModel :: lst ->
            {prevModel with UndoList = lst; CurrentKeyPresses = []}, Cmd.batch [sheetCmd DoNothing] // global helper function

    | KeyPress CtrlY ->
        match model.RedoList with
        | [] -> model , Cmd.none
        | newModel :: lst -> { newModel with UndoList = model :: model.UndoList; RedoList = lst; CurrentKeyPresses = []} , Cmd.batch [sheetCmd DoNothing]

    | KeyPress CtrlA ->
        let symbols = model.Wire.Symbol.Symbols |> Map.toList |> List.map fst
        let wires = model.Wire.Wires |> Map.toList |> List.map fst
        { model with
            SelectedComponents = symbols
            SelectedWires = wires
        } , Cmd.batch [ symbolCmd (SelectSymbols symbols) // global helper function
                        wireCmd (SelectWires wires) ] // global helper function

    | SaveSymbols ->
        model, symbolCmd SymbolT.SaveSymbols

    | WireType Radiussed ->
        let wires = model.Wire.Wires |> Map.toList |> List.map fst
        model,
        Cmd.batch [
            wireCmd (UpdateWireDisplayType Radial) // global helper function
            wireCmd (MakeJumps (false,wires)) // global helper function
        ]

    | ColourSelection (compIds, connIds, colour) ->
        {model with SelectedComponents = compIds; SelectedWires = connIds},
        Cmd.batch [
            symbolCmd (ColorSymbols (compIds, colour))  // global helper function
            wireCmd (ColorWires (connIds, colour)) // global helper function
        ]

    | ResetSelection ->
        {model with SelectedComponents = []; SelectedWires = []},
        Cmd.batch [
            symbolCmd (SelectSymbols []) // global helper function
            wireCmd (SelectWires []) // global helper function
        ]

    | _ -> 
        printfn "other cases not shown, see Sheet.fs in Issie for the full code"
        model, Cmd.none
    |> fun (model, (cmd: Cmd<ModelType.Msg>)) -> {issieModel with Sheet = model}, cmd

let extracted2MoveSymbols (model: SheetT.Model) (mMsg: MouseT) =
    
    let nextAction, isDragAndDrop = // grouping 
        match model.Action with
        | DragAndDrop -> DragAndDrop, true
        | _ -> MovingSymbols, false 
    
    let notIntersectingComponents (model: SheetT.Model) (box1: BoundingBox) (inputId: CommonTypes.ComponentId) = // function wrapping
        model.BoundingBoxes
        |> Map.filter (fun sId boundingBox -> boxesIntersect boundingBox box1 && inputId <> sId)
        |> Map.isEmpty

    match model.SelectedComponents with
    | [] -> model, Cmd.none
    | [symId] -> 

        let compId = model.SelectedComponents.Head
        let snapXY, moveDelta =
            // some code here that calculates snapXY, moveDelta
            // following result is made up so the code compiles
            // this is irrelevant to the purpose of this global function demonstration anyways
            {SnapX = model.SnapSymbols.SnapX; SnapY = model.SnapSymbols.SnapY}, {X=1.; Y=2.}
        let bBox = model.BoundingBoxes[compId]
        let errorComponents  =
            if notIntersectingComponents model bBox compId then [] else [compId]

        {model with
            Action = nextAction
            SnapSymbols = snapXY
            LastMousePos = mMsg.Pos
            ScrollingLastMousePos = {Pos=mMsg.Pos;Move=mMsg.ScreenMovement}
            ErrorComponents = errorComponents},
        Cmd.batch [ symbolCmd (MoveSymbols (model.SelectedComponents, moveDelta))  // global helper function
                    sheetCmd (UpdateSingleBoundingBox model.SelectedComponents.Head)     // global helper function
                    symbolCmd (ErrorSymbols (errorComponents,model.SelectedComponents,isDragAndDrop))  // global helper function
                    sheetCmd CheckAutomaticScrolling // global helper function
                    wireCmd (BusWireT.UpdateWires (model.SelectedComponents, moveDelta))] // global helper function

    | _ -> 
        let errorComponents =
            model.SelectedComponents
            |> List.filter (fun sId -> not (notIntersectingComponents model model.BoundingBoxes[sId] sId)) 
        {model with Action = nextAction;
                    LastMousePos = mMsg.Pos; 
                    ScrollingLastMousePos = {Pos=mMsg.Pos;Move=mMsg.ScreenMovement}; 
                    ErrorComponents = errorComponents},
        Cmd.batch [ symbolCmd (SymbolT.MoveSymbols (model.SelectedComponents, mMsg.Pos - model.LastMousePos)) // global helper function
                    symbolCmd (SymbolT.ErrorSymbols (errorComponents,model.SelectedComponents,isDragAndDrop)) // global helper function
                    sheetCmd UpdateBoundingBoxes // global helper function
                    sheetCmd CheckAutomaticScrolling // global helper function
                    wireCmd (BusWireT.UpdateWires (model.SelectedComponents, mMsg.Pos - model.LastMousePos))] // global helper function


