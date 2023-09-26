// -------------------------------- 9. Adding Parameters -------------------------------- //

// Where two blocks of code can't immediately be commoned up as a value or function used twice,
// adding a parameter can make this possible. Choose function and parameter name and operation so that what it does is clear.

module addingParam

// Here WebExtensions have 2 functions that calculate areas. They do a similar thing.
let calculateRectangleArea width height =
    width * height

let calculateTriangleArea baseLen height =
    (baseLen * height) / 2



type ShapeType =
    | Rectangle
    | Triangle
// By adding an extra parameter we can unify the 2 functions and create 1 more versetile function.
// We have 1 less function definition and hence a less cluttered codebase.
let calculateArea shapeType width height =
    match shapeType with
    | Rectangle -> width * height
    | Triangle  -> (width * height) / 2

// This makes an interesting point. One of the problems of working on a large codebase such as Issie
// is that there exist too many functions that do a lot of different things and sometimes it is hard to keep track of them.
// By having more versetile functions that can be used in more scenarios, you gain in readability and maintainability.