// -------------------------------- 3. Input Wrapping -------------------------------- //

// Use local let definitions to wrap input parameters transforming them into a form that
// can be used with less duplication (and helpful names) multiple times in expressions.
// Suppose you want to get a list of names and generate hello messages for a certain number of them.
module inputWrapping

// Before Input Wrapping
let changeListUpToIndex1 num (list: string list) =
    
    // Generate different types of questions or greetings based on the index
    let hellos = list |> List.mapi (fun idx el -> if idx <= num - 1 then "Hi " + el + " !" else el)
    let q1 = list |> List.mapi (fun idx el -> if idx <= num - 1 then el + " where are you from?" else el)
    let q2 = list |> List.mapi (fun idx el -> if idx <= num - 1 then el + " what is your favourite colour?" else el)
      
    // Return the lists
    hellos, q1, q2



// After input wrapping
let changeListUpToIndex2 num (list: string list) =
    // Adjust the index to be zero-based to fit list indexing. (Input Wrapping)
    let i = num - 1
    
    // Generate different types of questions or greetings based on the index
    let hellos = list |> List.mapi (fun idx el -> if idx <= i then "Hi " + el + " !" else el)
    let q1 = list |> List.mapi (fun idx el -> if idx <= i then el + " where are you from?" else el)
    let q2 = list |> List.mapi (fun idx el -> if idx <= i then el + " what is your favourite colour?" else el)
      
    // Return the lists
    hellos, q1, q2


// This might not be the best possible example since most people will wonder "Why didn't he just use 'if idx < num' ?"
// but this is just to prove the point of wrapping the input of a function into some more usefull local definition that
// allows you to use it frequently inside the function, avoiding repetition (subtracting 1 every time for example).