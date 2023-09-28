module DrawHelpers
open Fable.React.Props
open CommonTypes


//-------------------------------------------------------------------------//
//------------------------------Types--------------------------------------//
//-------------------------------------------------------------------------//




type PortLocation = {
    X: float
    Y: float
    R: float
}

type MouseOp = 
    /// button up
    | Up
    /// button down
    | Down
    /// Move with button up
    | Move 
    /// Move with button Down
    | Drag

type MouseT = {
    /// DrawBlock coords (scaled from screen pixels by 1/zoom)
    Pos: XYPos
    /// movement in screen pixel coords
    ScreenMovement: XYPos
    /// position on screen in screen pixel coords
    ScreenPage: XYPos
    ShiftKeyDown: bool
    Op: MouseOp}

/// Record to help draw SVG circles
type Circle = {
    ///  Radius of the circle
    R: float  
    /// color of outline: default => black color
    Stroke: string
    /// width of outline: default => thin
    StrokeWidth: string
    /// Fill: 0.0 => transparent, 1.0 => opaque
    FillOpacity: float // transparent fill
    /// color of fill: default => black color
    Fill: string
}

/// Record tonhelp draw SVG lines
type Line = {
    /// color of outline: default => black color
    Stroke: string
    /// width of outline: default => thin
    StrokeWidth: string
    /// what type of line: default => solid
    StrokeDashArray: string
}


/// Record to help create SVG paths (for wire segment jumps ONLY)
type Path = {
    Stroke: string
    StrokeWidth: string
    StrokeDashArray: string
    StrokeLinecap: string
    Fill: string
}

/// Record to help create SVG polygons
type Polygon = {
    Stroke: string
    StrokeWidth: string
    FillOpacity: float
    Fill: string
}

/// Record to help create SVG text
type Text = {
    /// start/end/middle: horizontal algnment vs (X,Y)
    TextAnchor: string
    FontSize: string
    FontWeight: string
    FontFamily: string
    Fill: string
    UserSelect: UserSelectOptions
    /// auto/middle/hanging: vertical alignment vs (X,Y)
    DominantBaseline: string
}


/// Default line, change this one to create new lines
let defaultLine = {
    Stroke = "Black"
    StrokeWidth = "1px"
    StrokeDashArray = "None"
}

/// Default path, change this one to create new paths
let defaultPath = {
    Stroke = "Black"
    StrokeWidth = "1px"
    StrokeDashArray = "None"
    StrokeLinecap = "butt"
    Fill = "transparent"
}

/// Default polygon, change this one to create new polygons
let defaultPolygon = {
    Stroke = "Black"
    StrokeWidth = "1px"
    FillOpacity = 1.0
    Fill = "None"
}

/// Default circle, change this one to create new circles
let defaultCircle = {
    R = 5.0
    Stroke = "Black"
    StrokeWidth = "1px"
    FillOpacity = 1.0
    Fill = "None"
}

/// Default text, change this to create new text types
let defaultText = {
    TextAnchor = "middle"
    FontSize = "10px"
    FontFamily = "verdana"
    FontWeight = "normal"
    Fill = "black"
    UserSelect = UserSelectOptions.None
    DominantBaseline = "hanging"
}

/// Port circle, used by both Sheet and Symbol to create ports
let portCircle = { defaultCircle with R = 5.0; Stroke = "Black"; StrokeWidth = "1.0px"; Fill = "Grey"}
let portCircleTarget= { defaultCircle with R = 8.0; Stroke = "DodgerBlue"; StrokeWidth = "2.0px"; Fill = "None"}

/// HLP23 AUTHOR: BRYAN TAN
/// Custom component corner circle
let cornerCircle = { defaultCircle with R = 5.0; Stroke = "Black"; StrokeWidth = "1.0px"; Fill = "Red"}


//--------------------------------------------------------------------------//
//-----------------------------Helpers--------------------------------------//
//--------------------------------------------------------------------------//



/// return a v4 (random) universally unique identifier (UUID)
/// works under .NET and FABLE
#if FABLE_COMPILER
let uuid():string = import "v4" "uuid"
#else
let uuid():string = System.Guid.NewGuid.ToString()
#endif


//--------------------------------Constants----------------------------------//


/// Calculates if two bounding boxes intersect by comparing corner coordinates of each box
let boxesIntersect (box1: BoundingBox) (box2: BoundingBox) =
    // Requires min and max since H & W can be negative, i.e. we don't know which corner is which automatically
    // Boxes intersect if there is overlap in both x and y coordinates 
    min box1.TopLeft.X (box1.TopLeft.X + box1.W) < max box2.TopLeft.X (box2.TopLeft.X + box2.W)
    && min box2.TopLeft.X (box2.TopLeft.X + box2.W) < max box1.TopLeft.X (box1.TopLeft.X + box1.W)
    && min box1.TopLeft.Y (box1.TopLeft.Y + box1.H) < max box2.TopLeft.Y (box2.TopLeft.Y + box2.H)
    && min box2.TopLeft.Y (box2.TopLeft.Y + box2.H) < max box1.TopLeft.Y (box1.TopLeft.Y + box1.H)



    

