# **Five Principles for Code Quality**

1. **DRY** (Don't Repeat Yourself). All programmers know this. All good programmers try to do it. In functional programming the power of functional abstraction means you can implement it more often than you think!
2. **NEP**. (No Exceptions Possible). Exceptions that *might happen* in your code cause hard to find bugs. In F# you can write code with zero exceptions. Typically you can allow documented exceptions (using **failwithf** ) only where it is obvious from the code *locally* that those exceptions cannot happen. If you have made an error the **failwithf** message should then make it quick to find it.
3. **DUI**. Document Using Identifiers (and XML comments). Choosing identifiers that make your code readable matters. There are four principles:
   1. Identifiers must not include noise - information that is unhelpful in the code context and makes the identifier longer than necessary. This argues for short identifiers, especially when they are repeated in expressions.
   2. Identifiers must document the code. They should say **what** they are, or **what** the function does, not **how** it does it, or **how** it will be used. This argues for long identifiers, especially when what they are or do is not clear from context.
   3. Identifier size should vary with scope and number of uses. Local identifiers can be single letter if a longer name does not help understanding given the context. Global names should document what a function does or a value is, however a helper function used everywhere might have a shorter name to aid expression readability and because it will be more easily remembered.
   4. XML comments are visible by hovering on refrences. They should add detail about what (not how) to make using a function or value easy. They will usually be 2 lines long, but can range from 1 - 5 lines. You can add extra documentation (e.g. how the function works when this is needed) as non-XML comments inside a function definition.
4. **RSN** (Reduce Syntactic Noise). Make it easier to see what really matters about the code without unnecessary boilerplate. Pipelines are one obvious example of this, as is type inference which allows unnecessary types to be omitted. Types should be included only when they make code more readable by documenting what functions do, typically for inputs and outputs of global helpers and larger functions.
5. **LFM** (Layout Follows Meaning). We read code visually, so visual cues – what lines up with what, what is next to what, determine how quick it is to identify what code does. An example is always indenting match case expressions to the same place on a new line so that the pattern/expression links are visually emphasised. Layout can sometimes provide useful hints about meaning (e.g. what is grouped with what). The order in which things are written affects the ease with which we process them. Pipelines, for example,  address both order (made to follow data) and visual queues (each pipeline element is aligned vertically. So a *trivial* syntactic rearrangement can make code muhc more readable.

# **Why do these principles matter?**

Most of programmer's time is spent trying to understand and refactor other people's code. In large code bases the speed and reliability with which this is possible determines productivity. In addition, even writing code for the first time, using functional abstraction to avoid repetition makes code faster to debug.

Programming requires good **algorithms** , **types** , and code **quality**. The length of Knuth's classic and excellent [The Art of computer Programming](https://en.wikipedia.org/wiki/The_Art_of_Computer_Programming) shows the depth of knowledge required to use **algorithms** effectively. In FP choosing the correct **types** to implement data structures is typically done first and very important. Having chosen algorithms, and types, writing code of high **quality** makes an enormous difference, especially for code maintenance.

Better algorithms,  better structure, all also improve code quality. Those things are hard work to get right, and very time-consuming to retrofit. The things in this document are easy to learn and implement and can be safely retrofitted to improve code. They apply to a wide variety of programs.

Although these transforms are mostly "robotic" - which to implement, and how to implement it, is a matter of judgement. There are so many possibilities, some of which deliver more benefit than others (in fact some of which positively harm code readability). Choosing good names, or the optimal function boundaries for useful functional abstraction,  requires a high level of skill, a detailed understanding of the program semantics, and an organised approach so that names and code structure is consistent.

# **Seven Useful Ways To Transform Code**

Each of these categories of transformation represents identifiable code which can be transformed and (usually) made better. In some cases the transformation is nearly always beneficial. In others it is a matter of judgement. Thinking about how you might apply these transformations to your code is the way to write better code. Practice and it will become very fast.

The transformations are grouped loosely as below.

Writing good code is about more than these transformations. For example, the correct F# types must be identified and written, correct code must be written. Still in assessment of project individual code quality many in the class typically get low marks because of code that is very long and difficult to read, which could be simplified using these transforms. 

1. **Functional (and let value definition) abstraction**

_This is more important than all the other transformations. Choosing structural abstraction with the right function boundaries determines how a problem is solved - for example: types can be seen as what is needed to encode function inputs and outputs meaningfully. Functional abstraction is also the key way to implement_ _ **DRY** __._

_Each of the transformations here can be considered and implemented whenever it is appropriate._

[Structural Abstraction](fsharp_examples/fsharp_examples/StructuralAbstraction.fs). Partition code into functions each doing part of the work. Special case: [structural pipelining](fsharp_examples/fsharp_examples/Pipelining.fs). Special and common combination of pipelining and structural abstraction where a function is split into multiple pipelined subfunctions each of which does part of the work. This corresponds to transforming the input data through a number of stages. DUI (with pipelining, add LFM).

[Input Wrapping](fsharp_examples/fsharp_examples/InputWrapping.fs). Use local `let` definitions to wrap input parameters transforming them into a form that can be used with less duplication (and helpful names) multiple times in expressions (DRY, DUI, RSN)

[Match case compression 1](fsharp_examples/fsharp_examples/MatchCaseCompression.fs). Move common code inside multiple `match` case expressions outside before the match. Note that this may require the `match` case expressions (or part of them) to be turned into functions so that the "variable" bits can be factored out. But often all that is needed is let definitions before the `match` (DRY).

[Match case compression 2](fsharp_examples/fsharp_examples/MatchCaseCompression.fs). Move common code at the end of multiple `match` case expressions *after* the match – pipelining the `match` result into a common function.

[Function wrapping](fsharp_examples/fsharp_examples/FunctionWrapping.fs). A function is used multiple times in a particular way – e.g. with some inputs fixed, or with a consistent change to its output. Create a local wrapper function which implements the modified function. Note that this is similar in effect to input wrapping. Note also that if inputs are corrctly ordered Currying does this without the need for a wrapper - although a wrapper may still be used to reduce noise and add abstraction (DRY, DUI). The value of this depends on how much *mess* it simplifies and whether the wrapper function makes sense and has a suitable name.

[Helper functions](fsharp_examples/fsharp_examples/GlobalHelperFunction.fs). Identify and code globally useful helper functions (DRY, DUI)

2. **Parametrization**

[Ordering](fsharp_examples/fsharp_examples/ParameterOrdering.fs). Order parameters so that Currying can be used nicely to implement partial evaluation. Generally this means the more static ones first. Note that in some cases there are no good orders, and an anonymous adapter function is needed to feed the correct argument in from a pipeline. (RSN).

[Grouping](fsharp_examples/fsharp_examples/ParameterGrouping.fs) (see records). Group related function parameters together as a single compound parameter. This can be done with tuples, but is usually best done with records - always ask whether your tuples would be better implemented as records which document fields with field names. (DUI, RSN).

[Static parameter elimination](fsharp_examples/fsharp_examples/StaticParamElimination.fs) This is about choosing function scope. One motivation for making a function a subfunction *local* is that static information it needs available in the outer function can be fed in directly as in-scope identifiers without the overhead of passing it through function parameters. This overhead is typically not about performance. Compilers can optimsie that. It is cognitive processing overhead matching up parameters. (RSN).

[Adding a parameter](fsharp_examples/fsharp_examples/AddingParameters.fs). Where two blocks of code can't immediately be commoned up as a value or function used twice, adding a parameter can make this possible. Choose function and parameter name and operation so that what it does is clear.

3. **Pipelines**

[Making](fsharp_examples/fsharp_examples/Pipelining.fs). A pipeline, in code, is motivated by LFM. The order of operations in a pipeline follows the actual order in which data is transformed - so turning function applications into pipelines makes reading easier. (LFM).

[Breaking](fsharp_examples/fsharp_examples/BreakingPipeline.fs). A pipeline may lead to code that is dificult to understand if its stages do not make sense individually. In that case breaking the pipeline with a let defined identifier that documents an intermediate value can make code simpler. (DUI).

[Anonymous function at end](fsharp_examples/fsharp_examples/AnonymousFunAtEnd.fs). FP allows a pipeline to be formed with an anonymous function at the end of it. If you think about it, this is identical to a let definition followed an expression using the let-defined value(s). the choice of which form is best should be made based on readability: it is often not clear-cut which form is better - don't assume the "more functional" one is better! (LFM).

4. **Records**

Records, with discriminated unions, are the key types you use to describe your data. In addition records (or anonymous records, if used one-off) can be used as below to transform your code into a more readable form.

[Grouping definitions](fsharp_examples/fsharp_examples/Records.fs): Mutiple repeated name prefixes or suffixes: `cursorX`, `cursorY` make code difficult to read. The correct answer is nearly always a record type.Possibly, if used only in one case, an anonymous record. DRY

[Grouping parameters](fsharp_examples/fsharp_examples/Records.fs). Special case of grouping definitions which is particularly important since too many parameters make function calls difficult to read as well as complicating function definitions. A great example of this would be the common practice of using a single *configuration* record (DRY, RSN).

5. **Match**

*Match lists*. The great advantage of `match` on lists is that the first one or several elements can be extracted as separate identifiers, and if this is not possible a suitable alternative thing can be done. So `match` combines check for list length and list indexing in away that obeys NEP (no exceptions possible). (NEP, RSN).

*Matching on tuples* (DRY).Matching on tuples can lead to very concise and readable code becaue wildcards can be used where a tuple is not matched. Unlike `when` guards, which break the automatic pattern completion check - because the compiler cannot work out when a `when` guard will allow a pattern to match - tuple matching makes it easy to see if you have left a case out. (NEP, RSN)

*Using wildcards*. Use them ( **\_** in patterns) to increase readability. If the unmatched part should be named to aid readability use **\_name** which tells the compiler you are not using name. (RSN).

*Using guards*. A long `if then elif then ... else then` expression can always be simplified to a `match` on `()` with all patterns wildcards (always match) and a `when` guard for each of the `if` conditions. That is perhaps a bit extreme, but often a single `when` guard can allow a complex nested `if` to be coded as a single `match` in a way that is more readable. (RSN, LFM).

*Definitional (including `as`)*. Match patterns are powerful because they combine matching with `let` definitions: each pattern identifier is effectively the same as a `let` definition. The matched expression can be any expression. Sometimes you want to use both the parts of a pattern *and* the whole thing. The **as** keyword allows this - warning - it can get ugly if you need brackets to ensure the **as** binds to the cirrect bit of the pattern. (RSN, DUI).

*Use discriminated unions* **.** A D.U. has named values - matching on D.U. cases is usually much more readable than matching on booleans. There is an overhead in having too many type defintions: but D.U. types can be defined private in a module or submodule in which case they are not visible outside that. Use two-value D.U.s as substitute for booleans when the type value names add readability and the type is used in multiple places.

6. **Monadic operations**

When processing monadic values (`Option`, `Result`) you have the choice of using **match** to separate the cases, or the following monadic functions. Often (though not always) the functions make what you are doing more readable, as well as (always) being more concise. (RSN, NEP): 
Typical use cases are in pipelines through which `Option` or `Result` values pass transformed by `map` or `bind` monadic functions. Note that such a pipeline can also use the `function | ...` form of `match` to transform data where this is needed: but the standard functions are preferable where they work.

- `Option.map` / `Result.map`.
- `Option.bind` / `Result.bind`
- `Option.orElse`
- `Option.defaultValue`

7. **Exceptions**

Nearly all F# exceptions come from using the following unsafe constructs:

- `Reduce` (fails with no elements - use `fold instead)
- Indexing (into lists or arrays or maps): `a[1]`
- `List.head`, `List.tail`
- `Option.get`

In all these cases there is a **try** version of the function which is safe and can be used instead. Or, see **match** , a match expression can be used to ensure safety. (NEP)
