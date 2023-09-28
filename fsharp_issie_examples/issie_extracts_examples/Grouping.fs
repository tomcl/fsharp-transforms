// ----------------------------------- Grouping ----------------------------------- //

// Group related function parameters together as a single compound parameter.
// This can be done with tuples, but is usually best done with records -
// always ask whether your tuples would be better implemented as records.

// A good example of this functions that take props type inputs in tuples

module Grouping

open CommonTypes

// setups for the extracts
let inline getCompRotatedHAndW (comp: Component) (transform: STransform) hScale vScale  =
    let hS,vS = (Option.defaultValue 1.0 hScale),(Option.defaultValue 1.0 vScale) // grouping
    match transform.Rotation with
    | Degree0 | Degree180 -> comp.H*vS, comp.W*hS
    | Degree90 | Degree270 -> comp.W*hS, comp.H*vS

let beforeCleanUpVer comp transform hScale vScale =
    let h = fst (getCompRotatedHAndW comp transform hScale vScale) // without grouping 
    let w = snd (getCompRotatedHAndW comp transform hScale vScale) // without grouping 
    let centreX = comp.X + w/2. // without grouping 
    let centreY = comp.Y + h/2. // without grouping 
    {X=centreX;Y=centreY}

let extracted comp transform hScale vScale =
    let h,w = getCompRotatedHAndW comp transform hScale vScale // grouping
    let centreX, centreY = comp.X + w/2., comp.Y + h/2. // grouping
    {X=centreX;Y=centreY}
