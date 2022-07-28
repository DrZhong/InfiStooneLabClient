import { ReagentDto, ReagentServiceProxy, CommonServiceProxy, CompanyType, StorageAttrEnum, LocationDto, DictDto, ReagentCatalog } from './../../../../shared/service-proxies/service-proxies';
import { Component, Injector, Input, OnInit, ViewChild } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { finalize, map, filter, delay } from 'rxjs/operators';
import { SFComponent, SFSchema, SFSchemaEnum, SFSelectWidgetSchema } from '@delon/form';
import { of } from 'rxjs';

@Component({
  selector: 'app-edit-reagent',
  templateUrl: './edit-reagent.component.html',
  styles: [
  ]
})
export class EditReagentComponent extends ModalComponentBase {

  constructor(
    private reagentServiceProxy: ReagentServiceProxy,
    private commonServiceProxy: CommonServiceProxy,
    injector: Injector) {
    super(injector);
  }

  @Input() id: number | undefined;
  @Input() record: ReagentDto;

  loading = false;
  // clientConfirm = false;
  // doubleConfirm = false;
  save(v: ReagentDto) {
    if (v.reagentLocationIds.length == 0) {
      this.notify.warn('请选择存放位置');
      return;
    }
    this.loading = true;
    v.capacityUnit = this.record.capacityUnit;

    if (v.reagentCatalog == ReagentCatalog.专管试剂) {
      v.doubleConfirm = this.record.doubleConfirm;
      v.clientConfirm = this.record.clientConfirm;
    }

    if (this.record.id) {
      //update
      this.reagentServiceProxy.update(v)
        .pipe(finalize(() => {
          this.loading = false;
        })).subscribe((x) => {
          abp.notify.success('修改成功');
          this.success(1);
        });
    } else {
      //create
      this.reagentServiceProxy.create(v)
        .pipe(finalize(() => {
          this.loading = false;
        })).subscribe((x) => {
          abp.notify.success('添加成功');
          this.success(1);
        });
    }


  }

  locations: LocationDto[] = [];
  filterdLocation: LocationDto[] = [];
  ngOnInit(): void {

    if (this.id) {
      this.schema.properties.reagentCatalog.readOnly = true;
    }
    this.reagentServiceProxy.getForEdit(this.id)
      .subscribe(res => {

        this.record = res;
        this.init();
      });

  }

  capacityUnits: DictDto[] = [];
  init() {
    this.commonServiceProxy.getLocation()
      .subscribe(res => {
        this.locations = res;
        this.filterdLocation = res;
        if (this.record.storageAttr) {
          this.selectedStorageAttr = this.record.storageAttr;
          this.filterLocation();
          //this.sf.refreshSchema();
        }
      });

    this.commonServiceProxy.getCapacityUnitSelectList()
      .subscribe(res => {
        this.capacityUnits = res;
        if (!this.record.capacityUnit) {
          if (res.length > 0) {
            this.record.capacityUnit = res[0].name;
          }
        }
      });
  }

  selectedStorageAttr = 0;
  filterLocation() {
    let filterLocation = this.locations
      .filter(q => q.locationStorageAttr
        .findIndex(wc => wc.storageAttr == this.selectedStorageAttr && wc.isActive) > -1);
    this.filterdLocation = filterLocation;
    //console.log(this.filterdLocation);

    const statusProperty = this.sf.getProperty('/reagentLocationIds')!;
    statusProperty.schema.enum = this.filterdLocation.map(src => {
      return {
        label: src.name, value: src.id
      }
    });
    statusProperty.widget.reset(this.record.reagentLocationIds);

  }

  @ViewChild('sf', { static: false }) private sf: SFComponent;
  schema: SFSchema = {
    type: "object",
    ui: { grid: { xs: 24, md: 12 }, spanLabelFixed: 120, ingoreKeywords: [] },
    properties: {
      no: {
        type: 'string',
        title: '编号'
      },
      reagentStatus: {
        title: '状态',
        type: 'number',
        default: 0,
        enum: [
          { label: '液态', value: 0 },
          { label: '固态', value: 1 },
          { label: '气态', value: 2 },
        ],
        ui: {
          widget: 'select',
        },
      },
      reagentCatalog: {
        type: 'number',
        title: '类型',
        enum: [
          { label: '常规试剂', value: 0 },
          { label: '标品试剂', value: 1 },
          { label: '专管试剂', value: 2 }
        ],
        readOnly: false,
        ui: {
          widget: 'select',
        },
        default: 2,
      },
      confirmArea: {
        type: 'string',
        title: '出入库确认',
        ui: {
          widget: 'custom',
          visibleIf: { reagentCatalog: [2] }
        }
      },
      casNo: {
        type: 'string',
        title: 'CAS号',
        maxLength: 64
      },

      cnName: {
        title: '中文名',
        type: "string",
        ui: {
          grid: { span: 24 }
        }
      },
      cnAliasName: {
        title: '中文别名',
        type: "string",
        ui: {
          grid: { span: 24 }
        }
      },
      enName: {
        title: '英文名',
        type: "string",
        ui: {
          grid: { span: 24 }
        }
      },
      safeAttribute: {
        title: '安全属性',
        type: 'number',
        enum: [
          { label: '易制毒', value: 0 },
          { label: '易制爆', value: 1 },
          { label: '剧毒品', value: 2 }
        ],
        //description: '如果试剂类型是专管试剂，则必须选择一种安全属性！',
        ui: {
          widget: 'select',
          allowClear: true,
          optionalHelp: '如果试剂类型是专管试剂，则必须选择一种安全属性！',
          placeholder: '--请选择--'
        }
      },
      price: {
        title: '参考价格',
        type: 'number'
      },
      storageAttr: {
        title: '存储属性',
        type: "string",
        ui: {
          widget: 'select',
          placeholder: '--请选择--',
          asyncData: () => {
            return this.commonServiceProxy
              .getStorageAttrList()
              .pipe(map(w => {
                return w.map(src => {
                  return {
                    label: src.desction, value: src.enumValue
                  };
                })
              }))
          },
          change: (ngModel: number) => {

            this.selectedStorageAttr = ngModel;
            this.record.reagentLocationIds = [];
            this.filterLocation();
          }
        },
      },
      pinYinCode: {
        title: '拼音码',
        type: "string",
      },
      reagentLocationIds: {
        title: '存放位置',
        type: "string",
        //enum: [''],
        ui: {
          grid: { span: 24 },
          mode: 'tags',
          widget: 'select',
          placeholder: '请先选择【存储属性后】再选择存放位置！'
        } as SFSelectWidgetSchema,
      },
      purity: {
        title: '纯度',
        type: "string",
        ui: {
          widget: 'select',
          placeholder: '--请选择--',
          asyncData: () => {
            return this.commonServiceProxy
              .getPuritySelectList()
              .pipe(map(w => {
                return w.map(src => src.name)
              }))
          }
        }
      },
      capacity: {
        title: '容量',
        type: "string",
        ui: {
          widget: 'custom',

        }
      },
      storageCondition: {
        title: '存储条件',
        type: "string",
        ui: {
          widget: 'select',
          placeholder: '--请选择--',
          asyncData: () => {
            return this.commonServiceProxy
              .getStorageConditionSelectList()
              .pipe(map(w => {
                return w.map(src => src.name)
              }))
          }
        }
      },
      inventoryWarning: {
        title: '库存预警',
        type: 'integer',
        ui: {
          addOnAfter: '瓶',
          optionalHelp: '设置0为永不预警！'
        }
      },
      supplierCompanyId: {
        title: '供应商',
        type: "string",
        ui: {
          widget: 'select',
          placeholder: '--请选择--',
          asyncData: () => {
            return this.commonServiceProxy
              .getAllCompany(true)
              .pipe(map(w => {
                return w
                  .filter(q => q.companyType == CompanyType.供应商)
                  .map(src => {
                    return {
                      label: src.name, value: src.id
                    };
                  })
              }))
          }
        }
      },
      productionCompanyId: {
        title: '生产商',
        type: "string",
        ui: {
          widget: 'select',
          placeholder: '--请选择--',
          asyncData: () => {
            return this.commonServiceProxy
              .getAllCompany(true)
              //.pipe(filter(x => x.findIndex(src => src.companyType == CompanyType.供应商) > -1))
              .pipe(map(w => {
                return w
                  .filter(q => q.companyType == CompanyType.生产商)
                  .map(src => {
                    return {
                      label: src.name, value: src.id
                    };
                  })
              }))
          }
        }
      },
    },
    required: ['no', 'reagentLocationIds', 'reagentCatalog', 'reagentStatus', 'cnName', 'storageAttr', 'pinYinCode', 'purity', 'capacity', 'storageCondition', 'inventoryWarning', 'supplierCompanyId', 'productionCompanyId']
  };

}
