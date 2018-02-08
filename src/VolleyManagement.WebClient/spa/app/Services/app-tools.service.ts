import { Injectable } from '@angular/core';

@Injectable()
export class AppToolsService {

    private _css3Supported: boolean = null;
    private _appMetadataFile: any;

    constructor() { }

    isCss3Supported(): boolean {
        if (this._css3Supported === null) {
            this._css3Supported = this._checkCss3Support();
        }

        return this._css3Supported;
    }

    getAppMetadataFileName(): string {
        if (!this._appMetadataFile) {
            this._appMetadataFile = document.getElementsByTagName('vm-app')[0].getAttribute('metadatafile');
        }

        return this._appMetadataFile;
    }

    private _checkCss3Support(): boolean {
        let propertyToCheck = 'border-radius';
        const div = document.createElement('div');
        const vendors = 'Khtml Ms O Moz Webkit'.split(' ');
        let len = vendors.length;
        let result = false;

        if (propertyToCheck in div.style) {
            result = true;
        }

        propertyToCheck = propertyToCheck.replace(/^[a-z]/, function (val) {
            return val.toUpperCase();
        });

        while (len--) {
            if (vendors[len] + propertyToCheck in div.style) {
                result = true;
            }
        }
        return result;
    }
}
