const storeImageDimensions = (e: Event): void => {
    const $heightInput = (document.querySelector('#MapTiles_Height') as HTMLInputElement);
    const $widthInput = (document.querySelector('#MapTiles_Width') as HTMLInputElement);

    if (!$heightInput || !$widthInput) {
        return;
    }

    e.preventDefault();

    const draftBtn = document.querySelector('button[value="submit.Publish"]') as HTMLButtonElement;
    const submitBtn = document.querySelector('button[value="submit.Publish"]') as HTMLButtonElement;

    draftBtn.disabled = true;
    submitBtn.disabled = true;

    const src = (document.querySelector('#mediaContainerMain .thumb-container img') as HTMLImageElement).src.split('?')[0]
    const img = new Image();

    img.onload = () => {
        if ($heightInput) {
            $heightInput.value = img.height.toString();
        }

        if ($widthInput) {
            $widthInput.value = img.width.toString();
        }

        draftBtn.disabled = false;
        submitBtn.disabled = false;
    }

    img.src = src;
};

export default storeImageDimensions;