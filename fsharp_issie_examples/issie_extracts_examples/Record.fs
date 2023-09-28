


module Record

open CommonTypes
open DrawModelType.BusWireT
open BlockHelpers
open BusWire
open Optics

//------------------------------autoroute--------------------------------------//

/// Contains geometric information of a port
type PortInfo = {
    Edge: Edge
    Position: XYPos
}

/// Returns a PortInfo object given a port edge and position
let inline genPortInfo edge position =
    { Edge = edge; Position = position }

/// Returns an edge rotated 90 degrees anticlockwise
let inline rotate90Edge (edge: Edge) = 
    match edge with
    | CommonTypes.Top -> CommonTypes.Left
    | CommonTypes.Left -> CommonTypes.Bottom
    | CommonTypes.Bottom -> CommonTypes.Right
    | CommonTypes.Right -> CommonTypes.Top

/// Returns a port rotated 90 degrees anticlockwise about the origin
let inline rotate90Port (port: PortInfo) =
    let newEdge = rotate90Edge port.Edge

    let newPos =
        { X = port.Position.Y
          Y = -port.Position.X }

    genPortInfo newEdge newPos

/// Returns a function to rotate a segment list 90 degrees about the origin, 
/// depending on its initial orientation
let rotateSegments90 initialOrientation =
    let horizontal i =
        match initialOrientation with
        | Horizontal -> i % 2 = 0
        | Vertical -> i % 2 = 1

    let rotateSegment (i, seg) =
        if (horizontal i) then
            { seg with Length = -seg.Length }
        else
            seg

    List.indexed
    >> List.map rotateSegment

/// Returns a version of the start and destination ports rotated until the start edge matches the target edge.
let rec rotateStartDest (target: Edge) ((start, dest): PortInfo * PortInfo) = 
    if start.Edge = target then
        (start, dest)
    else
        rotateStartDest target (rotate90Port start, rotate90Port dest)


/// Gets a wire orientation given a port edge
let inline getOrientationOfEdge (edge: Edge) = 
    match edge with
    | CommonTypes.Top | CommonTypes.Bottom -> Vertical
    | CommonTypes.Left | CommonTypes.Right -> Horizontal


/// Returns an anonymous record containing the starting symbol edge of a wire and its segment list that has been 
/// rotated to a target symbol edge.
let rec rotateSegments (target: Edge) (wire: {| edge: Edge; segments: Segment list |}) =
    if wire.edge = target then
        {| edge = wire.edge; segments = wire.segments |}
    else
        let rotatedSegs =
            rotateSegments90 (getOrientationOfEdge wire.edge) wire.segments
        
        {| edge = rotate90Edge wire.edge; segments = rotatedSegs |}
        |> rotateSegments target 


/// Returns a newly autorouted version of a wire for the given model
let autoroute (model: Model) (wire: Wire) : Wire =
    let destPos, startPos =
        Symbol.getTwoPortLocations (model.Symbol) (wire.InputPort) (wire.OutputPort)

    let destEdge =
        getInputPortOrientation model.Symbol wire.InputPort

    let startEdge =
        getOutputPortOrientation model.Symbol wire.OutputPort

    let startPort = genPortInfo startEdge startPos
    let destPort = genPortInfo destEdge destPos
    
    // Normalise the routing problem to reduce the number of cases in makeInitialSegmentsList
    let normStart, normEnd = 
        rotateStartDest CommonTypes.Right (startPort, destPort)

    let initialSegments =
        makeInitialSegmentsList wire.WId normStart.Position normEnd.Position normEnd.Edge

    let segments =
        {| edge = CommonTypes.Right
           segments = initialSegments |}
        |> rotateSegments startEdge // Rotate the segments back to original orientation
        |> (fun wire -> wire.segments)

    { wire with
          Segments = segments
          InitialOrientation = getOrientationOfEdge startEdge
          StartPos = startPos
    }