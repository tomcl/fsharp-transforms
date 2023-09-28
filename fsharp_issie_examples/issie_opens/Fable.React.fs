namespace Fable.React

open System

type [<AllowNullLiteral>] ReactElement =
    interface end

type ReactElementType =
    interface end

type ReactElementType<'props> =
    inherit ReactElementType

type IRefValue<'T> =
    abstract current: 'T with get, set

type IContext<'T> =
    interface end

type ISSRContext<'T> =
    inherit IContext<'T>
    abstract DefaultValue: 'T

type IReactExports =
    /// Create and return a new React element of the given type. The type argument can be either a tag name string (such as 'div' or 'span'), a React component type (a class or a function), or a React fragment type.
    abstract createElement: comp: obj * props: obj * children: ReactElement seq -> ReactElement

    /// Creates a Context object. When React renders a component that subscribes to this Context object it will read the current context value from the closest matching Provider above it in the tree.
    abstract createContext: defaultValue: 'T -> IContext<'T>

    /// React.createRef creates a ref that can be attached to React elements via the ref attribute.
    abstract createRef: initialValue: 'T -> IRefValue<'T>

    /// React.forwardRef creates a React component that forwards the ref attribute it receives to another component below in the tree.
    abstract forwardRef: fn: ('props -> IRefValue<'T> option -> ReactElement) -> ReactElementType<'props>

    /// If your component renders the same result given the same props, you can wrap it in a call to React.memo for a performance boost in some cases by memoizing the result. This means that React will skip rendering the component, and reuse the last rendered result.
    abstract memo: render: ('props -> ReactElement) * areEqual: ('props -> 'props -> bool) -> ReactElementType<'props>

    /// The React.Fragment component lets you return multiple elements in a render() method without creating an additional DOM element.
    abstract Fragment: ReactElementType<obj>

    /// React.Suspense lets you specify the loading indicator in case some components in the tree below it are not yet ready to render. In the future we plan to let Suspense handle more scenarios such as data fetching.
    abstract Suspense: ReactElementType<obj>

    /// React.startTransition lets you mark updates inside the provided callback as transitions. This method is designed to be used when React.useTransition is not available.
    /// Requires React 18.
    abstract startTransition: callback: (unit -> unit) -> unit

    /// The React version.
    abstract version: string

type FragmentProps = { key: string }

type Fragment(props: FragmentProps) =
    interface ReactElement
