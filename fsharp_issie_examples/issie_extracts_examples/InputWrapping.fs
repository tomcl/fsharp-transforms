// -------------------------------- Input Wrapping -------------------------------- //

// Use local let definitions to wrap input parameters transforming them into a form that
// can be used with less duplication (and helpful names) multiple times in expressions.

module InputWrapping

open CommonTypes

type BoundingBox = 
    {
        /// Top left corner of the bounding box
        TopLeft: XYPos
        /// Width
        W: float
        /// Height
        H: float
    }

let boxUnion (box:BoundingBox) (box':BoundingBox) =
    let maxX = max (box.TopLeft.X + box.W) (box'.TopLeft.X + box'.W)
    let maxY = max (box.TopLeft.Y + box.H) (box'.TopLeft.Y + box'.H)
    let minX = min box.TopLeft.X box'.TopLeft.X
    let minY = min box.TopLeft.Y box'.TopLeft.Y
    {
        TopLeft = {X = minX; Y = minY}
        W = maxX - minX
        H = maxY - minY
    }