class Utility {
    static toBase64(file) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => resolve(reader.result);
            reader.onerror = error => reject(error);
        });
    }

    static indexOfObjArray(array, key, value) {
        let result = -1;
        for(let index in Object.keys(array)) {
            if(array[index][key] === value) {
                result = index;
                break;
            }
        }
        return result;
    }
}