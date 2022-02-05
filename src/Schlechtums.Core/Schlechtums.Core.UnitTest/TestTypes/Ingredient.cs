using Schlechtums.Core.BaseClasses;
using Schlechtums.Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Schlechtums.Core.UnitTest.TestTypes
{
    [DebuggerDisplay("{Name}")]
    public class Ingredient : NotifyPropertyChanged
    {
        public Ingredient()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public Ingredient(string name, Dictionary<string, string> mviwValues)
            : this(name,
                  mviwValues["Grams Per Each"].ToNullableDecimal() ?? 0,
                   mviwValues["Grams Per Tsp"].ToNullableDecimal() ?? 0,
                   mviwValues["Dollars Per Each"].ToNullableDecimal() ?? 0,
                   mviwValues["Calories Per Each"].ToNullableDecimal() ?? 0,
                   mviwValues["Calories From Fat Per Each"].ToNullableDecimal() ?? 0,
                   mviwValues["Total Fat Per Each"].ToNullableDecimal() ?? 0,
                   mviwValues["Saturated Fat Per Each"].ToNullableDecimal() ?? 0,
                   mviwValues["Monounsaturated Fat Per Each"].ToNullableDecimal() ?? 0,
                   mviwValues["Polyunsaturated Fat Per Each"].ToNullableDecimal() ?? 0,
                   mviwValues["Cholesterol Per Each"].ToNullableDecimal() ?? 0,
                   mviwValues["Sodium Per Each"].ToNullableDecimal() ?? 0,
                   mviwValues["Total Carb Per Each"].ToNullableDecimal() ?? 0,
                   mviwValues["Sugar Per Each"].ToNullableDecimal() ?? 0,
                   mviwValues["Fiber Per Each"].ToNullableDecimal() ?? 0,
                   mviwValues["Protein Per Each"].ToNullableDecimal() ?? 0)
        { }

        internal Ingredient(string name, decimal gramsPerEach, decimal gramsPerTsp, decimal dollarsPerEach, decimal caloriesPerEach,
                            decimal caloriesFromFatPerEach, decimal totalFatPerEach, decimal satFatPerEach, decimal monoUnsatFatPerEach,
                            decimal polyUnsatFatPerEach, decimal cholesterolPerEach, decimal sodiumPerEach, decimal totalCarbPerEach,
                            decimal sugarPerEach, decimal fiberPerEach, decimal proteinPerEach)
            : this()
        {
            this.Name = name;
            this.GramsPerEach = gramsPerEach;
            this.GramsPerTsp = gramsPerTsp;

            if (this.GramsPerEach == -1)
                this.GramsPerEach = this.GramsPerTsp;

            if (gramsPerEach > 0)
            {
                this.DollarsPerGram = dollarsPerEach / gramsPerEach;
                this.CaloriesPerGram = caloriesPerEach / gramsPerEach;
                this.CaloriesFromFatPerGram = caloriesFromFatPerEach / gramsPerEach;
                this.TotalFatPerGram = totalFatPerEach / gramsPerEach;
                this.SatFatPerGram = satFatPerEach / gramsPerEach;
                this.MonoUnsaturatedFatPerGram = monoUnsatFatPerEach / gramsPerEach;
                this.PolyunsaturatedFatPerGram = polyUnsatFatPerEach / gramsPerEach;
                this.CholesterolPerGram = cholesterolPerEach / gramsPerEach;
                this.SodiumPerGram = sodiumPerEach / gramsPerEach;
                this.TotalCarbPerGram = totalCarbPerEach / gramsPerEach;
                this.SugarPerGram = sugarPerEach / gramsPerEach;
                this.FiberPerGram = fiberPerEach / gramsPerEach;
                this.ProteinPerGram = proteinPerEach / gramsPerEach;
            }
        }

        internal Ingredient(string name, string id, decimal gramsPerEach, decimal gramsPerTsp, decimal dollarsPerGram, decimal caloriesPerGram, decimal caloriesPerFatGram, decimal totalFatPerGram, decimal satFatPerGram, decimal monoUnsatFatPerGram, decimal polyUnsatFatPerGram, decimal cholesterolPerGram, decimal sodiumPerGram, decimal totalCarbPerGram, decimal sugarPerGram, decimal fiberPerGram, decimal proteinPerGram)
            : this(name, gramsPerEach, gramsPerTsp, dollarsPerGram * gramsPerEach, caloriesPerGram * gramsPerEach,
                   caloriesPerFatGram * gramsPerEach, totalFatPerGram * gramsPerEach, satFatPerGram * gramsPerEach,
                   monoUnsatFatPerGram * gramsPerEach, polyUnsatFatPerGram * gramsPerEach, cholesterolPerGram * gramsPerEach,
                   sodiumPerGram * gramsPerEach, totalCarbPerGram * gramsPerEach, sugarPerGram * gramsPerEach, fiberPerGram * gramsPerEach, proteinPerGram * gramsPerEach)
        {
            this.Id = id;
            this._IsRecipe = true;
        }

        internal bool _IsRecipe;
        private string _Id;
        public string Id
        {
            get { return this._Id; }
            set
            {
                if (this._Id != value)
                {
                    this._Id = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }

        private string _Name;
        public string Name
        {
            get { return this._Name; }
            set
            {
                if (this._Name != value)
                {
                    this._Name = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }

        private string _IngredientStringName;
        public string IngredientStringName
        {
            get { return this._IngredientStringName; }
            set
            {
                if (this._IngredientStringName != value)
                {
                    this._IngredientStringName = value;
                    this.RaisePropertyChanged(nameof(IngredientStringName));
                }
            }
        }

        private decimal _GramsPerEach;
        public decimal GramsPerEach
        {
            get { return this._GramsPerEach; }
            set
            {
                if (this._GramsPerEach != value)
                {
                    this._GramsPerEach = value;
                    this.RaisePropertyChanged("GramsPerEach");
                }
            }
        }

        private decimal _GramsPerTsp;
        public decimal GramsPerTsp
        {
            get { return this._GramsPerTsp; }
            set
            {
                if (this._GramsPerTsp != value)
                {
                    this._GramsPerTsp = value;
                    this.RaisePropertyChanged("GramsPerTsp");
                }
            }
        }

        private decimal _DollarsPerGram;
        public decimal DollarsPerGram
        {
            get { return this._DollarsPerGram; }
            set
            {
                if (this._DollarsPerGram != value)
                {
                    this._DollarsPerGram = value;
                    this.RaisePropertyChanged("DollarsPerGram");
                }
            }
        }

        private decimal _CaloriesPerGram;
        public decimal CaloriesPerGram
        {
            get { return this._CaloriesPerGram; }
            set
            {
                if (this._CaloriesPerGram != value)
                {
                    this._CaloriesPerGram = value;
                    this.RaisePropertyChanged("CaloriesPerGram");
                }
            }
        }

        private decimal _CaloriesFromFatPerGram;
        public decimal CaloriesFromFatPerGram
        {
            get { return this._CaloriesFromFatPerGram; }
            set
            {
                if (this._CaloriesFromFatPerGram != value)
                {
                    this._CaloriesFromFatPerGram = value;
                    this.RaisePropertyChanged("CaloriesFromFatPerGram");
                }
            }
        }

        private decimal _FatPerGram;
        public decimal TotalFatPerGram
        {
            get { return this._FatPerGram; }
            set
            {
                if (this._FatPerGram != value)
                {
                    this._FatPerGram = value;
                    this.RaisePropertyChanged("FatPerGram");
                }
            }
        }

        private decimal _SatFatPerGram;
        public decimal SatFatPerGram
        {
            get { return this._SatFatPerGram; }
            set
            {
                if (this._SatFatPerGram != value)
                {
                    this._SatFatPerGram = value;
                    this.RaisePropertyChanged("SatFatPerGram");
                }
            }
        }

        private decimal _MonoUnsaturatedFatPerGram;
        [DALSQLParameterName("MonoUnsatFatPerGram")]
        public decimal MonoUnsaturatedFatPerGram
        {
            get { return this._MonoUnsaturatedFatPerGram; }
            set
            {
                if (this._MonoUnsaturatedFatPerGram != value)
                {
                    this._MonoUnsaturatedFatPerGram = value;
                    this.RaisePropertyChanged("MonoUnsaturatedFatPerGram");
                }
            }
        }

        private decimal _PolyunsaturatedFatPerGram;
        [DALSQLParameterName("PolyUnsatFatPerGram")]
        public decimal PolyunsaturatedFatPerGram
        {
            get { return this._PolyunsaturatedFatPerGram; }
            set
            {
                if (this._PolyunsaturatedFatPerGram != value)
                {
                    this._PolyunsaturatedFatPerGram = value;
                    this.RaisePropertyChanged("PolyunsaturatedFatPerGram");
                }
            }
        }

        private decimal _CholesterolPerGram;
        public decimal CholesterolPerGram
        {
            get { return this._CholesterolPerGram; }
            set
            {
                if (this._CholesterolPerGram != value)
                {
                    this._CholesterolPerGram = value;
                    this.RaisePropertyChanged("CholesterolPerGram");
                }
            }
        }

        private decimal _SodiumPerGram;
        public decimal SodiumPerGram
        {
            get { return this._SodiumPerGram; }
            set
            {
                if (this._SodiumPerGram != value)
                {
                    this._SodiumPerGram = value;
                    this.RaisePropertyChanged("SodiumPerGram");
                }
            }
        }

        private decimal _TotalCarbPerGram;
        public decimal TotalCarbPerGram
        {
            get { return this._TotalCarbPerGram; }
            set
            {
                if (this._TotalCarbPerGram != value)
                {
                    this._TotalCarbPerGram = value;
                    this.RaisePropertyChanged("TotalCarbPerGram");
                }
            }
        }

        private decimal _SugarPerGram;
        public decimal SugarPerGram
        {
            get { return this._SugarPerGram; }
            set
            {
                if (this._SugarPerGram != value)
                {
                    this._SugarPerGram = value;
                    this.RaisePropertyChanged("SugarPerGram");
                }
            }
        }

        private decimal _FiberPerGram;
        public decimal FiberPerGram
        {
            get { return this._FiberPerGram; }
            set
            {
                if (this._FiberPerGram != value)
                {
                    this._FiberPerGram = value;
                    this.RaisePropertyChanged("FiberPerGram");
                }
            }
        }

        private decimal _ProteinPerGram;
        public decimal ProteinPerGram
        {
            get { return this._ProteinPerGram; }
            set
            {
                if (this._ProteinPerGram != value)
                {
                    this._ProteinPerGram = value;
                    this.RaisePropertyChanged("ProteinPerGram");
                }
            }
        }

        public decimal DollarsPerEach
        {
            get
            {
                return this.DollarsPerGram * this.GramsPerEach;
            }
            set
            {
                if (this.GramsPerEach > 0)
                    this.DollarsPerGram = value / this.GramsPerEach;
            }
        }

        public decimal CaloriesPerEach
        {
            get
            {
                return this.CaloriesPerGram * this.GramsPerEach;
            }
            set
            {
                if (this.GramsPerEach > 0)
                    this.CaloriesPerGram = value / this.GramsPerEach;
            }
        }

        public decimal CaloriesFromFatPerEach
        {
            get
            {
                return this.CaloriesFromFatPerGram * this.GramsPerEach;
            }
            set
            {
                if (this.GramsPerEach > 0)
                    this.CaloriesFromFatPerGram = value / this.GramsPerEach;
            }
        }

        public decimal TotalFatPerEach
        {
            get
            {
                return this.TotalFatPerGram * this.GramsPerEach;
            }
            set
            {
                if (this.GramsPerEach > 0)
                    this.TotalFatPerGram = value / this.GramsPerEach;
            }
        }

        public decimal SatFatPerEach
        {
            get
            {
                return this.TotalFatPerGram * this.GramsPerEach;
            }
            set
            {
                if (this.GramsPerEach > 0)
                    this.TotalFatPerGram = value / this.GramsPerEach;
            }
        }

        public decimal MonoUnsaturatedFatPerEach
        {
            get
            {
                return this.MonoUnsaturatedFatPerGram * this.GramsPerEach;
            }
            set
            {
                if (this.GramsPerEach > 0)
                    this.MonoUnsaturatedFatPerGram = value / this.GramsPerEach;
            }
        }

        public decimal PolyunsaturatedFatPerEach
        {
            get
            {
                return this.PolyunsaturatedFatPerGram * this.GramsPerEach;
            }
            set
            {
                if (this.GramsPerEach > 0)
                    this.PolyunsaturatedFatPerGram = value / this.GramsPerEach;
            }
        }

        public decimal CholesterolPerEach
        {
            get
            {
                return this.CholesterolPerGram * this.GramsPerEach;
            }
            set
            {
                if (this.GramsPerEach > 0)
                    this.CholesterolPerGram = value / this.GramsPerEach;
            }
        }

        public decimal SodiumPerEach
        {
            get
            {
                return this.SodiumPerGram * this.GramsPerEach;
            }
            set
            {
                if (this.GramsPerEach > 0)
                    this.SodiumPerGram = value / this.GramsPerEach;
            }
        }

        public decimal TotalCarbPerEach
        {
            get
            {
                return this.TotalCarbPerGram * this.GramsPerEach;
            }
            set
            {
                if (this.GramsPerEach > 0)
                    this.TotalCarbPerGram = value / this.GramsPerEach;
            }
        }

        public decimal SugarPerEach
        {
            get
            {
                return this.SugarPerGram * this.GramsPerEach;
            }
            set
            {
                if (this.GramsPerEach > 0)
                    this.SugarPerGram = value / this.GramsPerEach;
            }
        }

        public decimal FiberPerEach
        {
            get
            {
                return this.FiberPerGram * this.GramsPerEach;
            }
            set
            {
                if (this.GramsPerEach > 0)
                    this.FiberPerGram = value / this.GramsPerEach;
            }
        }

        public decimal ProteinPerEach
        {
            get
            {
                return this.ProteinPerGram * this.GramsPerEach;
            }
            set
            {
                if (this.GramsPerEach > 0)
                    this.ProteinPerGram = value / this.GramsPerEach;
            }
        }

        [DALIgnore]
        public string IgnoreThis { get; set; }
    }
}