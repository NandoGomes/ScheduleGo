namespace ScheduleGo.Shared.ScheduleGoContext.Contracts
{
	public interface ICalculatable<TType> where TType : ICalculatable<TType>
	{
		double ToDouble();

		public static double operator +(ICalculatable<TType> leftValue, ICalculatable<TType> rightValue) => leftValue.ToDouble() + rightValue.ToDouble();
		public static double operator -(ICalculatable<TType> leftValue, ICalculatable<TType> rightValue) => leftValue.ToDouble() - rightValue.ToDouble();
		public static double operator *(ICalculatable<TType> leftValue, ICalculatable<TType> rightValue) => leftValue.ToDouble() * rightValue.ToDouble();
		public static double operator /(ICalculatable<TType> leftValue, ICalculatable<TType> rightValue) => leftValue.ToDouble() / rightValue.ToDouble();

		public static double operator +(ICalculatable<TType> leftValue, double rightValue) => leftValue.ToDouble() + rightValue;
		public static double operator -(ICalculatable<TType> leftValue, double rightValue) => leftValue.ToDouble() - rightValue;
		public static double operator *(ICalculatable<TType> leftValue, double rightValue) => leftValue.ToDouble() * rightValue;
		public static double operator /(ICalculatable<TType> leftValue, double rightValue) => leftValue.ToDouble() / rightValue;

		public static double operator +(double leftValue, ICalculatable<TType> rightValue) => leftValue + rightValue.ToDouble();
		public static double operator -(double leftValue, ICalculatable<TType> rightValue) => leftValue - rightValue.ToDouble();
		public static double operator *(double leftValue, ICalculatable<TType> rightValue) => leftValue * rightValue.ToDouble();
		public static double operator /(double leftValue, ICalculatable<TType> rightValue) => leftValue / rightValue.ToDouble();
	}
}