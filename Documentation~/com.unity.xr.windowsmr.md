# About XR SDK for Windows MR

This package provides an XR SDK implementation of _Windows Mixed Reality_ support for Unity.

# Supported XR SDK Subsystems

Please see the [XR SDK documentation](https://github.cds.internal.unity3d.com/unity/xr.sdk) for information on all subsystems implemented here.

## Session

Subsystem implementation provides for initialization and management of the global _Windows Mixed Reality_ state. Also provides handling for handling pause/resume state changes.

This subsystem is required to successfully initialize before initializing any other subsystem defined below.

## Display

Subsystem implementation of the Display subsystem to allow for rendering of the VR view in the HMD or on HoloLens device.


## Input

Subsystem implementation of the Input subsystem to allow for tracking of device position/orientation, controller data, etc. Information provided to the subsystem from the provider is surfaced through [Tracked Pose Driver](https://docs.unity3d.com/ScriptReference/SpatialTracking.TrackedPoseDriver.html).

## Experience

Subsystem implementation provides for access to information about the current user experience. This includes the following:

* __Boundary Points__ - The boundary, as defined by the user, that describes the safe play area.
* __Experience Type__ - What type of reference frame the user is currently running within. The return values are:
    * __Local__ - Seated, device relative coordinates.
    * __Bounded__ - Standing experience limited to a bounded play area. Coordinates are relative to the "floor".
    * __Unbounded__ - Standing experience with no bounded play are limits. Coordinates are relative to the "floor".

## Gesture

Subsystem implementation to provide for recognition and tracking of gestures provided from the appropriate device.

See the relevant [Microsoft documentation](https://docs.microsoft.com/en-us/windows/mixed-reality/gestures) about Gestures for supported device information.

## Reference Point

Subsystem implementation provides support for ephemeral (non-stored) reference points, known as Anchors in official _Windows Mixed Reality_ documentation.

Successful initialization and start of the subsystem allows the user to Add reference points, Remove reference points and Query for all known reference points. Current subsystem definition does not provide for storage or retrieval of stored reference points.

See the relevant [Microsoft documentation](https://docs.microsoft.com/en-us/windows/mixed-reality/spatial-anchors) about Anchors for supported device information.

## Meshing

Subsystem implementation provides access to the meshing constructs the that HoloLens hardware produces. This subsystem only works on devices that actually support meshing (HoloLens) and should either be null or in a non-running state for other devices.

See the relevant [Microsoft documentation](https://docs.microsoft.com/en-us/windows/mixed-reality/spatial-mapping) about Spatial Mapping for supported device information as well as what to expect in regards to data from this subsystem.

# Additional support outside of XR SDK

There are a number of features that _Windows Mixed Reality_ supports that are not provided for in XR SDK. These are provided for use through the following extensions:

## Meshing Subsystem Extensions

Meshing subsystem provides only one means for setting a bounding volume for spatial mapping: __SpatialBoundingVolumeBox__. This API provides for settings a bounding volume as an Axis Aligned Bounding Box at a given position given specific extents. _Windows Mixed Reality_ additionally provides for setting a bounding volume as an Oriented Bounding Box, a Sphere or a Frustum.

* __SetBoundingVolumeOrientedBox__ - Similar to __SpatialBoundingVolumeBox__ but also allows for setting a given orientation to the volume.
* __SetBoundingVolumeSphere__ - Set a bounding volume to a sphere at some origin point and with the given radius.
* __SetBoundingVolumeFrustum__ - Set the bounding volume to the frustum defined by the 6 planes passed in. Each plane is defined as a point offset from the head, with a given orientation. The easiest way to set this is to use the [GeometryUtility.CalculateFrustumPlanes](https://docs.unity3d.com/ScriptReference/GeometryUtility.CalculateFrustumPlanes.html) Unity API and use that to populate the data for this call. The plane ordering passed in matches the plane ordering from this API.

## Reference Points Subsystem Extensions

# XR SDK Management support

While not required to use _Windows Mixed Reality_ XR SDK provider, integration with XR SDK Management provides for a simpler and easier way of using this (and other) providers within Unity. This package provides for the following XR SDK Management support:

* Runtime Settings - Provides for setting runtime settings to be used by the provider instance. These settings are per-supported platform.
* Build Settings - Provides for setting build settings to be used by the Unity build system. These settings are platform specific and are used to enable boot time settings as well as copy appropriate data to the build target.
* Lifecycle management - This package provides a default XR SDK Loader instance that can be used either directly or with the XR Management global manager. It provides for automated (or manual) lifetime management for all the subsystems that are implemented by this provider.
* Integration with Unity Settings UI - Custom editors and placement within the Unity Unified Settings UI within the top level XR settings area.

## Build Settings
* __Use Primary Window__ - Toggle to set the provider instance to immediately initialize XR SDK using the primary UWP window spawned by Unity. Set enabled by default. WSA/UWP Only.

## Runtime Settings

* __Shared Depth Buffer__ - Enabled or disable support for using a shared depth buffer. This allows Unity and the Mixed Reality Subsystem to use a common depth buffer. This allows the _Windows Mixed Reality_ system to provide better stabilization and integrated overlay support. Disabled by default.

* __Depth Buffer Format__ - Switch to determine the bit-depth of the depth buffer when sharing is enabled. Possible depth formats are _16bit_ and _24 bit_.

## XR SDK Management Loader

The default loader provided by the _Windows Mixed Reality_ XR SDK implementation is setup to use all the subsystems provided by this implementation. The only required subsystem is __Session__ which means that failure to initialize __Session__ will cause the loader to fail init and fall through to the next expected loader.

If __Session__ successfully initializes, then it is still possible for starting the subsystem could fail. If starting fails then the loader will clear all the subsystems and the app will fall through to standard Unity non-VR view.

All other subsystems depend on session but, unlike session, failure to initialize or start will not cause the whole provider to fail.

## Document revision history

|Date|Reason|
|---|---|
|March 25th, 2019 | Adding subsystem support and implementation details, XR SDK Management, extensions, etc. |
|December 13th, 2018 | Initial draft creation |


