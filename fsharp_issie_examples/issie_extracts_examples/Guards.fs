// -------------------------------- Guards -------------------------------- //

// A long if then elif then ... else then expression can always be simplified to a match on () 
// with all patterns wildcards (always match) and a when guard for each of the if conditions. 
// That is perhaps a bit extreme, but often a single when guard can allow a complex nested 
// if to be coded as a single match in a way that is more readable. (RSN, LFM).

module Guards

// setting up for example1
type Reducer ={
    mutable NextID: int
    mutable KeyMap: Map<string, int>
}

// example1
let withOutGuards1 (this:Reducer) (typ: string) (longId:string) =
    if typ = "C" || typ = "W" || typ = "P" then 
        if (longId.Length > 10) then 
            Map.tryFind longId this.KeyMap // this line has been modified and simplifed, does not affect the 
                                                        // demonstration of purpose of using guards here
        elif (longId[0] = typ[0]) then 
            None
        else 
            failwithf "{typ} is not a valid key type: 'C','W','P' are required for Component, Wire, or Port"
    else  
        failwithf "{typ} is not a valid key type: 'C','W','P' are required for Component, Wire, or Port"

let extractedFromIssieUsingGuards1 (this:Reducer) (typ: string) (longId:string) =
    match typ with
    | "C" | "W" | "P" when longId.Length > 10 ->  // using guards of '|' and "when" 
        Map.tryFind longId this.KeyMap
    | "C" | "W" | "P" when longId[0] = typ[0] ->  // using guards of '|' and "when" 
        None                
    | s -> failwithf "{s} is not a valid key type: 'C','W','P' are required for Component, Wire, or Port"

// example2
let withOutGuards2 (diffX:float) (diffY:float) normRot=
    let s:float = 1.0
    let lengthList() : float list = 
        if ((diffX >= 0) && (diffY >= 0)) then 
            match normRot with 
            | 0   -> [s; 0; diffX; diffY; 0; 0; -s]  
            | 90  -> [s; 0; (diffX - s)/2.0; (diffY + s); (diffX - s)/2.0; 0; 0; -s] 
            | 180 -> [s; 0; (diffX - 2.0 * s)/2.0; diffY; (diffX - 2.0 * s)/2.0; 0; s]
            | 270 -> [s; 0; (diffX - s); (diffY - s); 0; 0; 0; s]       
            | _   -> [s; 0; 0; 0; 0; 0; s]
        elif ((diffX >= 0) && (diffY < 0)) then 
            match normRot with 
            | 0   -> [s; 0; diffX; diffY; 0; 0; -s]  
            | 90  -> [s; 0; (diffX - s); (diffY + s); 0; 0; 0; -s]       
            | 180 -> [s; 0; (diffX - 2.0 * s)/2.0; diffY; (diffX - 2.0 * s)/2.0; 0; s]
            | 270 -> [s; 0; (diffX - s)/2.0; (diffY - s); (diffX - s)/2.0; 0; 0; s]
            | _   -> [s; 0; 0; 0; 0; 0; s]
        elif ((diffX < 0) && (diffY >= 0)) then 
            match normRot with 
            | 0   -> [s; 0; 0; diffY; diffX; 0; -s]
            | 90  -> [s; 0; 0; (diffY + s); (diffX - s); 0; 0; -s]  
            | 180 -> [s; diffY/2.0; (diffX - 2.0 * s); diffY/2.0; 0; 0; s] 
            | 270 -> [s; 0; 0; (diffY - s)/2.0; (diffX - s); (diffY - s)/2.0; 0; s] 
            | _   -> [s; 0; 0; 0; 0; 0; s]
        else 
            match normRot with 
            | 0   -> [s; 0; 0; diffY; diffX; 0; -s]
            | 90  -> [s; 0; 0; (diffY+s)/2.0; (diffX-s); (diffY+s)/2.0; 0; -s] 
            | 180 -> [s; diffY/2.0; (diffX - 2.0 * s); diffY/2.0; 0; 0; s] 
            | 270 -> [s; 0; 0; (diffY - s); (diffX - s); 0; 0; s]  
            | _   -> [s; 0; 0; 0; 0; 0; s]
    lengthList()

let extractedFromIssieUsingGuards2 (diffX:float) (diffY:float) normRot=
    let s:float = 1.0
    let lengthList() : float list = 
        match normRot with
        // Same orientation
        | 0 when (diffX >= 0) -> [s; 0; diffX; diffY; 0; 0; -s]                                                    
        | 0 when (diffX < 0) -> [s; 0; 0; diffY; diffX; 0; -s]                                             
        // Opposite orientation
        | 180 when (diffX >= 0) -> [s; 0; (diffX - 2.0 * s)/2.0; diffY; (diffX - 2.0 * s)/2.0; 0; s]           
        | 180 when (diffX < 0) -> [s; diffY/2.0; (diffX - 2.0 * s); diffY/2.0; 0; 0; s]            
        // Perpendicular orientation: if startPort points to the right, endPort points down
        | 90 when ((diffX >= 0) && (diffY >= 0)) -> [s; 0; (diffX - s)/2.0; (diffY + s); (diffX - s)/2.0; 0; 0; -s] 
        | 90 when ((diffX >= 0) && (diffY < 0)) -> [s; 0; (diffX - s); (diffY + s); 0; 0; 0; -s]                
        | 90 when ((diffX < 0) && (diffY >= 0)) -> [s; 0; 0; (diffY + s); (diffX - s); 0; 0; -s]               
        | 90 when ((diffX < 0) && (diffY < 0)) -> [s; 0; 0; (diffY+s)/2.0; (diffX-s); (diffY+s)/2.0; 0; -s]    
        // Perpendicular orientation: if startPort points to the right, endPort points up
        | 270 when ((diffX >= 0) && (diffY >= 0)) -> [s; 0; (diffX - s); (diffY - s); 0; 0; 0; s]         
        | 270 when ((diffX >= 0) && (diffY < 0)) -> [s; 0; (diffX - s)/2.0; (diffY - s); (diffX - s)/2.0; 0; 0; s] 
        | 270 when ((diffX < 0) && (diffY >= 0)) -> [s; 0; 0; (diffY - s)/2.0; (diffX - s); (diffY - s)/2.0; 0; s]   
        | 270 when ((diffX < 0) && (diffY < 0)) -> [s; 0; 0; (diffY - s); (diffX - s); 0; 0; s]  
        // Edge case that should never happen
        | _ -> [s; 0; 0; 0; 0; 0; s]
    lengthList()