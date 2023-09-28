module VerilogTypes

open Fable.React.Props

//////////////////////// Code Editor Types /////////////////////////////////

type State = 
    | Code of string

type CodeEditorProps = 
    | Placeholder of string
    | Value of string
    | OnValueChange of (string -> unit)
    | Highlight of (string -> obj)
    | TabSize of int
    | InsertSpaces of bool
    | IgnoreTabKey of bool
    | Padding of int
    | TextAreaId of string
    | TextAreaClassName of string
    | PreClassName of string
    |Style of CSSProp list


type CodeEditorOpen =
    |NewVerilogFile
    |UpdateVerilogFile of string
    
//////////////////////// Verilog Input Record   ///////////////////////////
type IdentifierT = {Name: string; Location: int}

type ModuleNameT = {Type : string; Name : IdentifierT}

type NumberT = {Type: string; NumberType: string; Bits: string option; Base: string option; UnsignedNumber: string option; AllNumber: string option; Location: int }

type RangeT = {Type: string; Start: string; End: string; Location: int}

type IOItemT = {Type: string; DeclarationType: string; Range : RangeT option; Variables: IdentifierT array; Location: int}

type ParameterT = {Type: string; Identifier: IdentifierT; RHS: NumberT}
type ParameterItemT = {Type: string; DeclarationType: string; Parameter : ParameterT;}

type PrimaryT = {Type: string; PrimaryType: string; BitsStart: string option; BitsEnd: string option; Primary: IdentifierT; Width: int option}

type ExpressionT = {Type: string; Operator: string option; Head: ExpressionT option; Tail: ExpressionT option; Unary: UnaryT option}
    and UnaryT = {Type: string; Primary: PrimaryT option; Number: NumberT option; Expression: ExpressionT option}

type AssignmentLHST = {Type: string; PrimaryType: string; BitsStart: string option; BitsEnd: string option; Primary: IdentifierT; VariableBitSelect: ExpressionT option; Width: int option}
type AssignmentT = {Type: string; LHS: AssignmentLHST; RHS: ExpressionT}

type ContinuousAssignT = {Type: string; StatementType: string; Assignment : AssignmentT; Location: int} // need to add seq block, option statement array

type DeclarationT = {Type: string; DeclarationType: string; Range: RangeT option; Variables: IdentifierT array; Location: int;}

type NonBlockingAssignT = {Assignment: AssignmentT}

type BlockingAssignT = {Operator: string; Assignment: AssignmentT}

type SeqBlockT = {Type: string; Statements: StatementT array; Location: int}

and StatementT = {Type: string; StatementType: string; NonBlockingAssign: NonBlockingAssignT option; BlockingAssign: BlockingAssignT option; SeqBlock: SeqBlockT option; Conditional: ConditionalT option; CaseStatement: CaseStatementT option; Location: int}

and IfStatementT = {Type: string; Condition: ExpressionT; Statement: StatementT; Location: int}

and ConditionalT = {Type: string; IfStatement: IfStatementT; ElseStatement: StatementT option; Location: int} 

and CaseItemT = {Type: string; Expressions: NumberT array; Statement: StatementT}

and CaseStatementT = {Type: string; Expression: ExpressionT; CaseItems: CaseItemT array; Default: StatementT option; Location: int}

type AlwaysConstructT = {Type: string; AlwaysType: string; Statement: StatementT; ClkLoc: int; Location: int}

type NamedPortConnectionT = {Type: string; PortId: IdentifierT; Primary: PrimaryT}

type ModuleInstantiationT = {Type: string; Module: IdentifierT; Identifier: IdentifierT; Connections: NamedPortConnectionT array}

type ItemT = {Type: string; ItemType: string; IODecl: IOItemT option; Decl: DeclarationT option; ParamDecl: ParameterItemT option; Statement: ContinuousAssignT option; AlwaysConstruct: AlwaysConstructT option; ModuleInstantiation: ModuleInstantiationT option; Location: int}

type ModuleItemsT = {Type : string; ItemList : ItemT array}

type ModuleT = {Type : string; ModuleName : IdentifierT; PortList : string array; Locations: string array; ModuleItems : ModuleItemsT; EndLocation: int;}

type VerilogInput = { Type:string; Module: ModuleT; }


////////////////////////////////////////////////////////////////////////////

type PortAssignmentError =
    |Unassigned
    |DoubleAssignment 

type ReplaceType =
    |IODeclaration
    |Assignment
    |Variable of string
    |NoReplace

type OneUnary = {Name:string;ResultWidth:int;Head:OneUnary option;Tail:OneUnary option;Elements:OneUnary list} 

type ExtraErrorInfo = {Text: string; Copy: bool; Replace: ReplaceType;}

type ErrorInfo = {Line:int; Col:int; Length: int; Message: string; ExtraErrors: ExtraErrorInfo array option}

type ParserOutput = {Result: string option; Error: ErrorInfo option; NewLinesIndex: int array option}
