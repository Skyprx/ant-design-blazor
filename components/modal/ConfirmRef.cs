﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AntDesign
{
    public class ConfirmRef<TResult> : ConfirmRef
    {
        public Func<TResult, Task> OnCancel { get; set; }

        public Func<TResult, Task> OnOk { get; set; }

        internal ConfirmRef(ConfirmOptions config, ModalService service) : base(config, service)
        {

        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <returns></returns>
        public async Task TriggerOkAsync(TResult result)
        {
            await (_service?.CloseAsync(this) ?? Task.CompletedTask);
            await (OnOk?.Invoke(result) ?? Task.CompletedTask);
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <returns></returns>
        public async Task TriggerCancelAsync(TResult result)
        {
            await (_service.CloseAsync(this) ?? Task.CompletedTask);
            await (OnCancel?.Invoke(result) ?? Task.CompletedTask);
        }
    }

    public class ConfirmRef
    {
        public ConfirmOptions Config { get; set; }

        protected ModalService _service;

        internal IModalTemplate ModalTemplate { get; set; }

        public Func<Task> OnOpen { get; set; }

        public Func<Task> OnClose { get; set; }

        public Func<Task> OnDestroy { get; set; }

        internal bool IsCreateByModalService => _service != null;

        internal ConfirmRef(ConfirmOptions config)
        {
            Config = config;
        }

        internal ConfirmRef(ConfirmOptions config, ModalService service)
        {
            Config = config;
            _service = service;
        }

        /// <summary>
        /// 打开窗体
        /// </summary>
        /// <returns></returns>
        public async Task OpenAsync()
        {
            await (_service?.OpenAsync(this) ?? Task.CompletedTask);
        }

        /// <summary>
        /// 关闭窗体无返回值
        /// </summary>
        /// <returns></returns>
        public async Task CloseAsync()
        {
            await (_service?.CloseAsync(this) ?? Task.CompletedTask);
        }

        public async Task UpdateConfig()
        {
            await (_service?.Update(this) ?? Task.CompletedTask);
        }

        public async Task UpdateConfig(ConfirmOptions config)
        {
            Config = config;
            await (_service?.Update(this) ?? Task.CompletedTask);
        }

        internal TaskCompletionSource<ConfirmResult> TaskCompletionSource { get; set; }
    }
}
