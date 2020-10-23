using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace PaymentCalculator
{
    class Calculator
    {
        private double _creditSum;
        private double _creditRate;
        private int _creditPeriod;
        private bool _creditType;
        public Calculator(double creditSum,double creditRate,int creditPeriod,bool creditType)
        {
            _creditSum = creditSum;
            _creditRate = creditRate;
            _creditPeriod = creditPeriod;
            _creditType = creditType;
        }
        public DataTable GetShedule()
        {
            DataTable dtShedule = new DataTable();
            dtShedule.Columns.Add("Месяц", typeof(int));
            dtShedule.Columns.Add("Сумма платежа", typeof(double));
            dtShedule.Columns.Add("Остаток по долгу", typeof(double));
            DataRow dr;
            if (_creditType == true)
            {
                double mainPayment = _creditSum / _creditPeriod;
                double tempCreditAmount = _creditSum;
                for(int i = 1; i<= _creditPeriod;i++)
                {
                    dr = dtShedule.NewRow();
                    double percent = tempCreditAmount * _creditRate;
                    double monthlyPayment = mainPayment+percent;
                    tempCreditAmount -= mainPayment;
                    dr[0] = i;
                    dr[1] = monthlyPayment;
                    dr[2] = tempCreditAmount;
                    
                    dtShedule.Rows.Add(dr);

                }
            }
            else if (_creditType == false)
            {
                double monthlyPayment = _creditSum * (_creditRate / (1 - Math.Pow(1 + _creditRate, -_creditPeriod)));
                double summaryCreditAmount = monthlyPayment * _creditPeriod;
                double tempCreditAmount = _creditSum;
                for(int i=1; i<=_creditPeriod;i++)
                {
                    dr = dtShedule.NewRow();
                    double percent = tempCreditAmount*_creditRate;
                    tempCreditAmount -= monthlyPayment-percent;
                    dr[0] = i;
                    dr[1] = monthlyPayment;
                    dr[2] = tempCreditAmount;
                    dtShedule.Rows.Add(dr);
                    
                }
            }
            return dtShedule;
        }
    }
}
