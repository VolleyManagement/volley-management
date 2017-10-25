import { Component, OnInit, Input} from '@angular/core';

@Component({
  selector: 'loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.scss']
})

export class LoaderComponent implements OnInit {
  @Input() isShowLoader: boolean;
  private isCss3Supported = false;

  constructor() { }

  ngOnInit() {
    this.isCss3Supported = this.checkCss3Support();
  }

  private checkCss3Support(): boolean {
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
