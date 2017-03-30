# Yumiko.SelfProtection

- ## Description
  - ### First (Generate new DLL for validation equipment)
    🍓 Uncomment Preprocessor Directives in [Strobarried.cs] like this
    ```C#
    #define Create_New
    ```
    
    🍓 Select the subject to attach to the DLL file
    ```C#
    var bios = new WMIProvider(WNISubject.Win32_BIOS);
    ```
    
    🍓 Compile it
    ```C#
    var strobarried = new Strobarried(bios);
    strobarried.Compile();
    ```
  
    🍓Press F5 to compile the project and now you can see the **Bind.dll** in directory after compiled

  - ### Second (Validate equipment)
    🍓 Comment Preprocessor Directives in [Strobarried.cs] like this
    ```C#
    //#define Create_New
    ```  
    
    🍓 Select the subject for comparison the DLL content
    ```C#
    var bios = new WMIProvider(WNISubject.Win32_BIOS);
    ```
    
    🍓 Validate it
    ```C#
    var strobarried = new Strobarried(bios);
    var validated = Strobarried.Validate(strobarried);
    ```

- ## License
  MIT
    
[Strobarried.cs]:<https://github.com/0x0001F36D/Yumiko.SelfProtection/blob/master/Yumiko.SelfProtection/Strobarried/Strobarried.cs>
