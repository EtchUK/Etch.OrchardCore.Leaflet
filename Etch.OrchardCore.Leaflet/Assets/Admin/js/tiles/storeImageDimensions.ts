const storeImageDimensions = (e: Event): boolean => {
    e.preventDefault();

    const draftBtn = document.querySelector('button[value="submit.Publish"]') as HTMLButtonElement;
    const submitBtn = document.querySelector('button[value="submit.Publish"]') as HTMLButtonElement;

    draftBtn.disabled = true;
    submitBtn.disabled = true;

    try {
        const src = (document.querySelector('#mediaContainerMain .thumb-container img') as HTMLImageElement).src.split('?')[0]
        const img = new Image();

        img.onload = () => {
            (document.querySelector('#MapTiles_Height') as HTMLInputElement).value = img.height.toString();
            (document.querySelector('#MapTiles_Width') as HTMLInputElement).value = img.width.toString();

            draftBtn.disabled = false;
            submitBtn.disabled = false;
        }

        img.src = src;
    }
    catch {
        return false;
    }

    return false;
};

export default storeImageDimensions;