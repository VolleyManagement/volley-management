import { Component, OnInit, Input } from '@angular/core';
import { AppToolsService } from '../../Services/app-tools.service';

@Component({
    selector: 'loader',
    templateUrl: './loader.component.html',
    styleUrls: ['./loader.component.scss']
})
export class LoaderComponent implements OnInit {

    @Input()
    visible: boolean;

    @Input()
    height: number;

    public isCss3Supported = false;

    constructor(private _appTools: AppToolsService) { }

    ngOnInit() {
        this.isCss3Supported = this._appTools.isCss3Supported();
    }
}
