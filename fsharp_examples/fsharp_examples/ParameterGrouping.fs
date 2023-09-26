// ----------------------------------- 8. Grouping ----------------------------------- //

// Group related function parameters together as a single compound parameter.
// This can be done with tuples, but is usually best done with records -
// always ask whether your tuples would be better implemented as records.

// A good example of this functions that take props type inputs in tuples

module grouping

// Without grouping
let createRectangle width height color isVisible =
    if isVisible then
        printfn "Creating a rectangle with Width: %d, Height: %d, Color: %s" width height color
    else
        printfn "The rectangle is not visible."

// Usage
createRectangle 10 20 "red" true


// With grouping, less cluttered definition.
let createRectangle2 props isVisible =
    let width, height, color = props
    if isVisible then
        printfn "Creating a rectangle with Width: %d, Height: %d, Color: %s" width height color
    else
        printfn "The rectangle is not visible."

// Usage
let props = (10, 20, "red")
createRectangle2 props true