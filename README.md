# Feliz.UseIsVisible
A Fable.React hook that determines whether a component is currently visible on the screen.

This hook is useful when you need to display a large amount and you only want to render rows that are visible on the screen for performance reasons.

## Example
In this example, 5000 rows are displayed, and the (vertical) margin is set to `400px` (which extends the "visible" range above and below the visible screen):

![useIsVisible400](https://user-images.githubusercontent.com/1030435/164080936-41549b1c-e95e-4e43-ae06-4fe469e3fdca.gif)


## Code

This approach requires that your dynamically rendered row be broken out into its own component that utilizes the `useIsVisible` hook:

```F#
open Feliz.UseIsVisible
```

### Using Feliz DSL

```F#
[<ReactComponent>]
let Row (entry: Monthly.EntryView) = 
    let rowRef = React.useElementRef()
    let isVisible = React.useIsVisible(rowRef, 400, [||])

    if isVisible then
        Html.tr [
            prop.ref rowRef
            prop.children [
                Html.td [ str entry.Username ]
                Html.td [ str (entry.Date.ToString("M/d/yyyy")) ]
                Html.td [ str entry.Task ]
                Html.td [ str (entry.Hours.ToString("0.00")) ]
                Html.td [ str entry.Project ]
                Html.td [ str entry.Email ]
                Html.td [ str "8:00 AM" ]
            ]
        ]
    else
        Html.tr [
            prop.ref rowRef            
            prop.children [
                Html.td [
                    prop.colSpan 7
                    prop.children [
                        Html.b [
                            str "Loading..."
                        ]
                    ]
                ]
            ]
        ]

```

### Using Fable.React DSL

```F#
[<ReactComponent>]
let Row (entry: Monthly.EntryView) = 
    let rowRef = React.useElementRef()
    let isVisible = React.useIsVisible(rowRef, 200, [||])

    if isVisible then
        tr [ Ref (adaptFelizUseElementRef rowRef) ] [
            td [] [ str entry.Username ]
            td [] [ str (entry.Date.ToString("M/d/yyyy")) ]
            td [] [ str entry.Task ]
            td [] [ str (entry.Hours.ToString("0.00")) ]
            td [] [ str entry.Project ]
            td [] [ str entry.Email ]
            td [] [ str "8:00 AM" ]
        ]
    else
        tr [ Ref (adaptFelizUseElementRef rowRef) ] [
            td [ ColSpan 7 ] [
                b [] [ str "Loading..." ]
            ]
        ]
```

## The Entire Code

```F#
module HoursWorkedPage

open System
open Feliz
open Fable.React
open Fable.React.Props
open Feliz.UseIsVisible

[<ReactComponent>]
let Row (entry: Monthly.EntryView) = 
    let rowRef = React.useElementRef()
    let isVisible = React.useIsVisible(rowRef, margin = 400)

    if isVisible then
        tr [ Ref (adaptFelizUseElementRef rowRef) ] [
            td [] [ str entry.Username ]
            td [] [ str (entry.Date.ToString("M/d/yyyy")) ]
            td [] [ str entry.Task ]
            td [] [ str (entry.Hours.ToString("0.00")) ]
            td [] [ str entry.Project ]
            td [] [ str entry.Email ]
            td [] [ str "8:00 AM" ]
        ]
    else
        tr [ Ref (adaptFelizUseElementRef rowRef) ] [
            td [ ColSpan 7 ] [
                b [] [ str "Loading..." ]
            ]
        ]

[<ReactComponent>]
let Page (company: Company) =
    let entries, setEntries = React.useState<Monthly.EntryView array>(Array.empty)
    
    React.useEffectOnce(fun () ->
        [|
            for n in [1 .. 5000] do
                {
                    Monthly.EntryView.Date = DateTime.Today
                    Monthly.EntryView.Email = "person@email.com"
                    Monthly.EntryView.Hours = 8
                    Monthly.EntryView.Project = $"Project %i{n}"
                    Monthly.EntryView.Task = $"Task %i{n}"
                    Monthly.EntryView.Username = $"User %i{n}"
                }
        |]
        |> setEntries
    )

    let export() = 
        printfn "Exporting...

    Ctrls.container [
        h4 [] [str "Hours Worked"]

        Ctrls.row [
            div [Class "col text-right"] [
                Button.commandBarButton [Button.IconProps {| iconName = "ExcelDocument" |}; Button.Title "Download Excel"; Button.OnClick (fun e -> export())] [
                    str "Export to Excel"
                ]                    
            ]
        ]
        Ctrls.row [
            Ctrls.col [
                table [Id "monthly-tbl"; Class "table"] [
                    thead [] [
                        tr [] [
                            th [Style [Width "100px"]] [str "BIM Detailer"]
                            th [Style [Width "70px"]] [str "Date"]
                            th [Style [Width "150px"]] [str "Task"]
                            th [Style [Width "70px"]] [str "Hours"]
                            th [Style [Width "150px"]] [str "Project"]
                            th [Style [Width "100px"]] [str "Email Address"]
                            th [Style [Width "100px"]] [str "Start Time"]
                        ]
                    ]
                    tbody [] [
                        for e in entries do
                            Row e
                    ]
                ]
            ]
        ]
    ]

```
