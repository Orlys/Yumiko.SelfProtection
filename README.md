# Yumiko.SelfProtection

## Description
- ### First (Generate new DLL for validation equipment)
  🍓 Uncomment Preprocessor Directives in [Strobarried.cs] like this
  ```C#
  #define Create_New
  ```

  🍓 Select the [subject](https://github.com/0x0001F36D/Yumiko.SelfProtection/blob/master/Yumiko.SelfProtection/WMI/WMISubject.cs "WMISubject.cs") to attach to the DLL file
  ```C#
  var bios = new WMIProvider(WNISubject.Win32_BIOS);
  ```
  
  🍓 Compile DLL
  ```C#
  var strobarried = new Strobarried(bios);
  strobarried.Compile();
  ```
  🍓Compile the project and now you can see the **Bind.dll** in directory after compiled
  🍓You need copied **Bind.dll** to your output folder


- ### Second (Append validation code into your code)
  🍓 Comment Preprocessor Directives in [Strobarried.cs] like this
  ```C#
  //#define Create_New
  ```
  🍓 Select the subject for comparison the DLL content
  ```C#
  var bios = new WMIProvider(WNISubject.Win32_BIOS);
  ```
  🍓 Validate Equipment
  ```C#
  var strobarried = new Strobarried(bios);
  var validated = Strobarried.Validate(strobarried);
  ```
## Reference
[WMI/MI/OMI Providers](https://msdn.microsoft.com/en-us/library/bg126473(v=vs.85).aspx "MSDN")

## License
MIT
    
[Strobarried.cs]:<https://github.com/0x0001F36D/Yumiko.SelfProtection/blob/master/Yumiko.SelfProtection/Strobarried/Strobarried.cs>
