/// Copied from https://github.com/Shmew/Fable.Extras
namespace Feliz.UseIsVisible

open Browser.Types
open Fable.Core
open FSharp.Core
open System

[<Erase;RequireQualifiedAccess>]
module JSe =

    [<Erase>]
    type DOMRectReadOnly =
        /// The x coordinate of the DOMRect's origin.
        [<Emit("$0.x")>]
        member _.X : float = jsNative
        
        /// The y coordinate of the DOMRect's origin.
        [<Emit("$0.y")>]
        member _.Y : float = jsNative
        
        /// The width of the DOMRect.
        [<Emit("$0.width")>]
        member _.Width : float = jsNative
        
        /// The height of the DOMRect.
        [<Emit("$0.height")>]
        member _.Height : float = jsNative
        
        /// Returns the top coordinate value of the DOMRect (usually the same as y.)
        [<Emit("$0.top")>]
        member _.Top : float = jsNative
        
        /// Returns the right coordinate value of the DOMRect (usually the same as x + width).
        [<Emit("$0.right")>]
        member _.Right : float = jsNative
        
        /// Returns the bottom coordinate value of the DOMRect (usually the same as y + height).
        [<Emit("$0.bottom")>]
        member _.Bottom : float = jsNative
        
        /// Returns the left coordinate value of the DOMRect (usually the same as x).
        [<Emit("$0.left")>]
        member _.Left : float = jsNative
        
    /// Describes the intersection between the target element and its root container at 
    /// a specific moment of transition.
    [<Erase>]
    type IntersectionObserverEntry =
        /// Returns the bounds rectangle of the target element as a DOMRectReadOnly.
        [<Emit("$0.boundingClientRect")>]
        member _.BoundingClientRect : DOMRectReadOnly = jsNative

        /// A number between 0.0 and 1.0 which indicates how much of the target element is actually 
        /// visible within the root's intersection rectangle. 
        ///
        /// More precisely, this value is the ratio of the area of the intersection rectangle (intersectionRect) 
        /// to the area of the target's bounds rectangle (boundingClientRect).
        /// 
        /// If the area of the target's bounds rectangle is zero, the returned value is 1 if isIntersecting is 
        /// true or 0 if not.
        [<Emit("$0.intersectionRatio")>]
        member _.IntersectionRatio : float = jsNative
        
        /// Returns a DOMRectReadOnly representing the target's visible area.
        [<Emit("$0.intersectionRect")>]
        member _.IntersectionRect : DOMRectReadOnly = jsNative
        
        /// A Boolean value which is true if the target element intersects with the intersection observer's root. 
        ///
        /// If this is true, then, the IntersectionObserverEntry describes a transition into a state of intersection; 
        /// if it's false, then you know the transition is from intersecting to not-intersecting.
        [<Emit("$0.isIntersecting")>]
        member _.IsIntersecting : bool = jsNative
        
        /// Returns a DOMRectReadOnly for the intersection observer's root.
        [<Emit("$0.rootBounds")>]
        member _.RootBounds : DOMRectReadOnly = jsNative
        
        /// The Element whose intersection with the root changed.
        [<Emit("$0.target")>]
        member _.Target : Element = jsNative
        
        /// The time elapsed (in ms) at which the intersection was recorded, relative to the 
        /// IntersectionObserver's time origin.
        [<Emit("$0.time")>]
        member _.Time : float = jsNative

    [<Erase;RequireQualifiedAccess>]
    module IntersectionObserverEntry =
        /// Returns the bounds rectangle of the target element as a DOMRectReadOnly.
        let inline boundingClientRect (observerEntry: IntersectionObserverEntry) = observerEntry.BoundingClientRect
        
        /// A number between 0.0 and 1.0 which indicates how much of the target element is actually 
        /// visible within the root's intersection rectangle. 
        ///
        /// More precisely, this value is the ratio of the area of the intersection rectangle (intersectionRect) 
        /// to the area of the target's bounds rectangle (boundingClientRect).
        /// 
        /// If the area of the target's bounds rectangle is zero, the returned value is 1 if isIntersecting is 
        /// true or 0 if not.
        let inline intersectionRatio (observerEntry: IntersectionObserverEntry) = observerEntry.IntersectionRatio
        
        /// Returns a DOMRectReadOnly representing the target's visible area.
        let inline intersectionRect (observerEntry: IntersectionObserverEntry) = observerEntry.IntersectionRect
        
        /// A Boolean value which is true if the target element intersects with the intersection observer's root. 
        ///
        /// If this is true, then, the IntersectionObserverEntry describes a transition into a state of intersection; 
        /// if it's false, then you know the transition is from intersecting to not-intersecting.
        let inline isIntersecting (observerEntry: IntersectionObserverEntry) = observerEntry.IsIntersecting
        
        /// Returns a DOMRectReadOnly for the intersection observer's root.
        let inline rootBounds (observerEntry: IntersectionObserverEntry) = observerEntry.RootBounds
        
        /// The Element whose intersection with the root changed.
        let inline target (observerEntry: IntersectionObserverEntry) = observerEntry.Target
        
        /// The time elapsed (in ms) at which the intersection was recorded, relative to the 
        /// IntersectionObserver's time origin.
        let inline time (observerEntry: IntersectionObserverEntry) = observerEntry.Time
    
    /// <summary>
    /// The IntersectionObserver provides a way to asynchronously observe changes 
    /// in the intersection of a target element with an ancestor element or with 
    /// a top-level document's viewport.
    /// </summary>
    /// <exception cref="System.Exception">
    /// The specified rootMargin is invalid.
    /// </exception>
    /// <exception cref="System.Exception">
    /// One or more of the values in threshold is outside the range 0.0 to 1.0.
    /// </exception>
    [<Erase>]
    type IntersectionObserver<'Root when 'Root :> Node and 'Root :> NodeSelector and 'Root :> GlobalEventHandlers> private () =        
        /// <param name="callback">
        /// A callback function to be run whenever a threshold is crossed in one direction or the other.
        /// </param>
        /// <param name="root">
        /// The document or element that is used as the viewport for checking visibility of the target. 
        ///
        /// Must be the ancestor of the target. 
        ///
        /// Defaults to the browser viewport if not specified.
        /// </param>
        /// <param name="rootMargin">
        /// Margin around the root. 
        ///
        /// Can have values similar to the CSS margin property, e.g. "10px 20px 30px 40px" (top, right, bottom, left). 
        ///
        /// The values can be percentages. 
        ///
        /// This set of values serves to grow or shrink each side of the root element's bounding box before computing 
        /// intersections. 
        ///
        /// Defaults to all zeros.
        /// </param>
        /// <param name="threshold">
        /// A sequence of numbers which indicate at what percentage of the target's visibility the observer's callback should be executed. 
        ///
        /// If you want the callback to run every time visibility passes another 25%, you would specify a seqeunce like: [0.; 0.25; 0.5; 0.75; 1.]. 
        ///
        /// The default is 0 (meaning as soon as even one pixel is visible, the callback will be run). 
        ///
        /// A value of 1.0 means that the threshold isn't considered passed until every pixel is visible.
        /// </param>
        [<Emit("new IntersectionObserver($0, {root: $1, rootMargin: $2, threshold: $3})")>]
        new (callback: IntersectionObserverEntry [] -> IntersectionObserver<'Root> -> unit, ?root: 'Root, ?rootMargin: string, ?threshold: seq<float>) = IntersectionObserver()
        
        /// The Element or Document whose bounds are used as the bounding box when testing for 
        /// intersection. 
        /// 
        /// If no root value was passed to the constructor or its value is None, the top-level 
        /// document's viewport is used.
        [<Emit("$0.root")>]
        member _.Root : 'Root option = jsNative
        
        /// An offset rectangle applied to the root's bounding box when calculating intersections, 
        /// effectively shrinking 
        /// or growing the root for calculation purposes. 
        ///
        /// The value returned by this property may not be the same as the one specified when 
        /// calling the constructor as it may be changed to match internal requirements. 
        ///
        /// Each offset can be expressed in pixels (px) or as a percentage (%). The default is 
        /// "0px 0px 0px 0px".
        [<Emit("$0.rootMargin")>]
        member _.RootMargin : string = jsNative
        
        /// A sequence of thresholds, sorted in increasing numeric order, where each threshold 
        /// is a ratio of intersection area to bounding box area of an observed target. 
        ///
        /// Notifications for a target are generated when any of the thresholds are crossed for 
        /// that target. 
        ///
        /// If no value was passed to the constructor, 0 is used.
        [<Emit("$0.thresholds")>]
        member _.Thresholds : seq<float> = jsNative

        /// Stops the IntersectionObserver object from observing any target.
        [<Emit("$0.disconnect()")>]
        member _.Disconnect () : unit = jsNative

        /// Tells the IntersectionObserver a target element to observe.
        [<Emit("$0.observe($1)")>]
        member _.Observe (element: #Element) : unit = jsNative

        /// Returns an array of IntersectionObserverEntry objects for all observed targets.
        [<Emit("$0.takeRecords()")>]
        member _.TakeRecords () : seq<IntersectionObserverEntry> = jsNative

        /// Tells the IntersectionObserver to stop observing a particular target element.
        [<Emit("$0.unobserve($1)")>]
        member _.Unobserve (element: #Element) : unit = jsNative

        /// Creates an IDisposable that disconnects the IntersectionObserver when disposed.
        member inline this.GetDisposable () = { new IDisposable with member _.Dispose () = this.Disconnect() }

    [<Erase;RequireQualifiedAccess>]
    module IntersectionObserver =
        /// The Element or Document whose bounds are used as the bounding box when testing for intersection. 
        /// 
        /// If no root value was passed to the constructor or its value is None, the top-level document's viewport is used.
        let inline root (observer: IntersectionObserver<'Root>) = observer.Root
        
        /// An offset rectangle applied to the root's bounding box when calculating intersections, effectively shrinking 
        /// or growing the root for calculation purposes. 
        ///
        /// The value returned by this property may not be the same as the one specified when calling the constructor as it 
        /// may be changed to match internal requirements. 
        ///
        /// Each offset can be expressed in pixels (px) or as a percentage (%). The default is "0px 0px 0px 0px".
        let inline rootMargin (observer: IntersectionObserver<'Root>) = observer.RootMargin
    
        /// A sequence of thresholds, sorted in increasing numeric order, where each threshold is a ratio of intersection area to bounding box 
        /// area of an observed target. 
        ///
        /// Notifications for a target are generated when any of the thresholds are crossed for that target. 
        ///
        /// If no value was passed to the constructor, 0 is used.
        let inline thresholds (observer: IntersectionObserver<'Root>) = observer.Thresholds
    
        /// Stops the IntersectionObserver object from observing any target.
        let inline disconnect (observer: IntersectionObserver<'Root>) = 
            observer.Disconnect()
            observer
    
        /// Tells the IntersectionObserver a target element to observe.
        let inline observe (observer: IntersectionObserver<'Root>) (element: #Element) = 
            observer.Observe(element)
            observer
    
        /// Returns an array of IntersectionObserverEntry objects for all observed targets.
        let inline takeRecords (observer: IntersectionObserver<'Root>) = observer.TakeRecords()
    
        /// Tells the IntersectionObserver to stop observing a particular target element.
        let inline unobserve (observer: IntersectionObserver<'Root>) (element: #Element) = 
            observer.Unobserve(element)
            observer
            
        /// Creates an IDisposable that disconnects the IntersectionObserver when disposed.
        let inline getDisposable (observer: IntersectionObserver<'Root>) = observer.GetDisposable()
