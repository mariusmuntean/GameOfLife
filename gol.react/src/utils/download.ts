export const download = (filename: string, text: string) => {
  var element = document.createElement("a");
  element.setAttribute("href", "data:text/plain;charset=utf-8," + encodeURIComponent(text));
  element.setAttribute("download", filename);

  element.style.display = "none";
  document.body.appendChild(element);

  element.click();

  document.body.removeChild(element);
};

export const pickFile = (onLoadedSuccessfully: (fileContent: string) => void) => {
  const filePickerInput = document.createElement("input");
  filePickerInput.type = "file";
  filePickerInput.id = "file";
  filePickerInput.className = "file-input";
  filePickerInput.accept = ".json";
  filePickerInput.style.display = "none";
  filePickerInput.onchange = (e) => {
    const filereader = new FileReader();
    filereader.onloadend = (ee) => {
      if (!ee) {
        return;
      }

      onLoadedSuccessfully(filereader.result as string);
    };
    filereader.readAsText((e.target as any)?.files?.[0]);
  };

  document.body.appendChild(filePickerInput);
  filePickerInput.click();
  document.body.removeChild(filePickerInput);
};
