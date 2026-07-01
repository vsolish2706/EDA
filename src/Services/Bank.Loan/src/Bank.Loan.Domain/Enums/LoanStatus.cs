using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Loan.Domain.Enums;

    public enum LoanStatus
    {
        Pending,
        UnderRiskEvaluation,
        Approved,
        Rejected,
        Disbursed,
        Cancelled
    }

