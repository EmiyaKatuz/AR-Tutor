### Hardware Setup Requirements:

1. Specific objects build for tracking

2. Any Mac devices with XCode installed

3. External camera or Apple's mobile devices

### Configuration Details:

1. If you want to track the real objects, you *MUST* import your customized object data into Unity and replace the
   existing ones. Here below is the detailed information for creating customized object data.

    - Multi Targets

   Firstly, design and prepare your images for each side of the Multi Target fulfilling both the matching dimensions and
   the ratings of a good Multi Target. The second step is to create the Multi-Target through the Vuforia Target Manager.
   For details, see <https://developer.vuforia.com/library/objects/multi-targets>.

    - Cylinder Targets

   Cylinder Targets are printed images that are wrapped onto conical or cylindrical objects. Therefore, you have to
   consider its dimensions and warp before applying the print to your object to ensure the Vuforia Engine can detect and
   steadily track the Cylinder Target. The preparation and design decisions include the shape and size of the physical
   object, the placement of the image on the object, and the potential image distortion from wrapping it around the
   object. When you have designed your target images, you can upload them to the Target Manager and add them as Cylinder
   Targets for image evaluation and rating. For details,
   see <https://developer.vuforia.com/library/objects/cylinder-targets>.

2. XCode 15.3+, iOS 15+, Unity Editor 2022 LTS 2022.3.16f1+ are required for iOS devices.

3. Windows 10 and 11, Unity Editor 2022 LTS 2022.3.16f1+, Visual Studio 2022 version 17.8.5+ are required for Windows. (
   The project is built on Unity 2022.3.40f1)

4. Developer Mode is a *MUST* for Apple's mobile devices.

5. Package `Vuforia Engine` and `Cone Mesh` are required.

6. Vuforia App License Key is required.

7. To build on a real iOS device, you will need a Apple developer account.

Once you get your data ready, download them from Vuforia Target Manager and import them to Unity. Select
`CylinderTarget` and `MultiTarget`, replace them to your object data. Then the tangible objects are set.

### Running Guide:

1. Running in Unity

   To test any function in Unity, you need to tick the test tick box in the Object `Result` first. Then, you can select
   the mode in the dropdown box `Mode Test` to test the specific mode you want. Click the `Play` button, then the
   program
   should work.

2. Running on mobile devices

   **Important: For mobile devices, we have built our app on Apple devices only. So, a MacBook with XCode installed is
   necessary for deploying the app on a real machine. Any other platforms are not tested and guaranteed to work
   properly.**

   Once you get your environment ready, then select Build and Run in Unity's Build Settings, the XCode will take over
   all the remaining. Plug your Apple mobile devices to the Mac, then   