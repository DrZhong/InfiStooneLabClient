import { Component, Injector, Input, OnInit } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { ReagentStockListDto } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-master-quickly-bind',
  templateUrl: './master-quickly-bind.component.html',
  styles: [
  ]
})
export class MasterQuicklyBindComponent extends ModalComponentBase implements OnInit {

  @Input()
  record: ReagentStockListDto;

  wsIinterval: any = undefined;
  constructor(injector: Injector) {
    super(injector);
  }
  num = 0;

  ngOnInit(): void {
    this.connectWs();
    this.wsIinterval = setInterval(() => {
      if (this.wsReady) return;
      this.connectWs();
    }, 30000);
  }
  wsReady = false;
  ws: WebSocket;//定义websocket
  //socket连接
  connectWs() {
    if (this.ws != null) { this.ws.close() };
    this.ws = new WebSocket("ws://127.0.0.1:18181/");
    let that = this;
    this.ws.onopen = (event) => {
      that.notify.success('打印机连接成功！');
      that.wsReady = true;
    };
    this.ws.onmessage = function (event: MessageEvent) {
      //socket 获取后端传递到前端的信息
      //that.ws.send('sonmething');
      //console.log(event.data);
      if (event.type == 'message') {
        var json = JSON.parse(event.data);
        if (json.success) {
          that.notify.success('正在打印...');
        } else {
          that.message.error(json.error);
        }
      }
    }
    this.ws.onerror = function (event) {
      //socket error信息
      that.notify.error('请检测打印软件是否启动！');

    }
    this.ws.onclose = function (event) {
      //socket 关闭后执行
      that.notify.info('打印软件已关闭！');
      that.wsReady = false;
    }
  }

  print(item: string) {
    if (!this.wsReady) {
      this.notify.error('请先打开打印机程序');
      return;
    }
    let val = {
      data: item
    };
    this.ws.send(JSON.stringify(val));
  }

  beforeClose() {
    clearInterval(this.wsIinterval);
    if (this.ws != null) { this.ws.close() };
    this.close();
  }


  save() {
    this.print(this.record.barCode);
  }

}
