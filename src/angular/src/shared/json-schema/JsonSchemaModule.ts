import { AppConsts } from '@shared/AppConsts';
import { NgModule } from '@angular/core';
import { DelonFormModule, WidgetRegistry } from '@delon/form';
import { SharedModule } from '@shared/shared.module';

export const SCHEMA_THIRDS_COMPONENTS = [
    // TinymceWidget,

];

@NgModule({
    declarations: SCHEMA_THIRDS_COMPONENTS,
    imports: [
        SharedModule,
        DelonFormModule.forRoot()
    ],
    exports: [
        ...SCHEMA_THIRDS_COMPONENTS
    ]
})
export class JsonSchemaModule {
    constructor(widgetRegistry: WidgetRegistry) {
        //widgetRegistry.register(TinymceWidget.KEY, TinymceWidget);
    }
}
