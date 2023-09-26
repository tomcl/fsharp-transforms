// -------------------------------- 14. & 15. Records -------------------------------- //

// Records, with discriminated unions, are the key types you use to describe your data.
// In addition records (or anonymous records, if used one-off) can be used as below
// to transform your code into a more readable form.
module records

// The first example we are going to see, demonstrates how records can be used to group
// definitions together in order to make them more intuitive and declutter code.

// Before using a record
let x = 1.0
let y = 2.0
let z = 3.0

// After using a record
type Point3D = { X: float; Y: float; Z: float }
let point = { X = 1.0; Y = 2.0; Z = 3.0 }



// The second example demonstrates grouping function parameters using a record.

// Before using a record
let calculateVolume length width height = length * width * height

let volume = calculateVolume 2.0 3.0 4.0

// After using a record
type BoxDimensions = { Length: float; Width: float; Height: float }

let calculateVolume2 dims = dims.Length * dims.Width * dims.Height

let dimensions = { Length = 2.0; Width = 3.0; Height = 4.0 }
let volume2 = calculateVolume2 dimensions