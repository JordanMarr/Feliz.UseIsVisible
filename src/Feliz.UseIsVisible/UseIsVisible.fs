namespace Feliz.UseIsVisible

open Feliz

[<AutoOpen>]
module UseIsVisible =

    /// Allows Fable.React.Props.Prop.Ref to use a Feliz IRefValue<'T> generated via the Feliz `React.useElementRef` hook.
    let adaptFelizUseElementRef (refValue: Fable.React.IRefValue<'T option>) (element: Browser.Types.Element) = 
        refValue.current <- 
            element 
            |> Option.ofObj 
            |> Option.map unbox<'T>

    type React with
        
        /// Determines whether a given element ref is visible on the screen. (Use `React.useElementRef ()` to get element input.)
        /// Margin extends the "visible" area to the given number of pixels above and below the actual viewable area.
        static member inline useIsVisible (element: IRefValue<Browser.Types.HTMLElement option>, ?margin: int, ?dependencies) =
            let isVisible, setIsVisible = React.useState(false)
            let margin = margin |> Option.defaultValue 0
            let dependencies = dependencies |> Option.defaultValue [||]

            React.useEffect (fun () ->
                match element.current with
                | Some el -> 
                    let observer = 
                        JSe.IntersectionObserver<Browser.Types.HTMLElement>(fun entries -> 
                            fun _ -> 
                                entries 
                                |> Array.tryHead
                                |> Option.iter (fun e -> 
                                    setIsVisible e.IsIntersecting
                                )
                        , rootMargin = $"%i{margin}px")

                    observer.Observe(el)
                    React.createDisposable (fun () -> observer.Unobserve(el))
                | None -> 
                    React.createDisposable id
            , dependencies)

            isVisible
