﻿using Evertec.Tips.Mobile.Domain.Exceptions;
using Evertec.Tips.Mobile.Interfaces;
using System.Collections.ObjectModel;
using Evertec.Tips.Domain.Mappers;
using Evertec.Tips.Domain.Models;
using Evertec.Tips.Infrastructure.Interfaces;

namespace Evertec.Tips.Mobile.Services
{
    public class TipService : ITipService
    {
        private ITipRepository _tipRepository;

        public TipService(ITipRepository tipRepository)
        {
            _tipRepository = tipRepository;
        }

        public Task<bool> DeleteTip(int id)
        {
            return _tipRepository.DeleteTip(id);
        }

        public async Task<bool> UpdateTip(TipModel tip)
        {
            var result = new bool();
            var responseValidate = tip.Validate();
            if (responseValidate.Result)
            {
                var tipEntity = await TipMapper.MapTipEntity(tip);
                result = await _tipRepository.UpdateTip(tipEntity);
            }
            else
                throw new AppException(responseValidate.CodeMessage);

            return result;
        }

        public async Task<bool> AddTip(TipModel tip)
        {
            var result = new bool();
            var responseValidate = tip.Validate();
            if (responseValidate.Result)
            {
                var tipEntity = await TipMapper.MapTipEntity(tip);
                result = await _tipRepository.AddTip(tipEntity);
            }
            else
                throw new AppException(responseValidate.CodeMessage);

            return result;
        }

        public async Task<ObservableCollection<TipModel>> GetAll()
        {
            var list = await _tipRepository.GetAll();
            return await TipMapper.MapTipsModel(list.OrderBy(i => i.CreationDate).ToList());
        }
    }
}
