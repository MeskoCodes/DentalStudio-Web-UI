using Services.Common.Dto;
using Services.Common.Dto.Account;

namespace Services.AccountService
{
    public interface IAccountService
    {
        Task<ObservableCollection<AccountDto>> GetAllAccounts();

        Task<AccountDto> GetAccountById(string accountId);

        Task<GeneralResponseDto> Update(string accountId, AccountUpdateDto account);

        Task<GeneralResponseDto> Delete(string accountId);
    }
}