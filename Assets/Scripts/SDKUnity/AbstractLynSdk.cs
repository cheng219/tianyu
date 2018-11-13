using System;
//using System.Collections;

public abstract class AbstractLynSdk {
	
	/** Result for login action. */
	public class LoginResult 
	{
		// No login action.
		public const int UNKOWN = 0;
		// Login success implicitly
		public const int SUCCESS_IMPLICIT = 1;
		// Login failed implicitly
		public const int FAILED_IMPLICIT = 11;
		// Login success explicitly by input user account and password.
		public const int SUCCESS_EXPLICIT = 2;
		// Login failed explicitly.
		public const int FAILED_EXPLICIT = 22;
	}
	
	/** Result of users' subscribed state about special packaged game.*/
	public class SubscribeResult {
		public const int UNKNOWN = 0;
		public const int YES = 1;
		public const int NO = 2;
	}
	
	/** Type of props about doBilling*/
	public class PropsType {
		public const int ONCE_ONLY = 1;
		public const int NORMAL = 2;
		public const int RIGHTS = 4;
	}
	
	/** Type of UI about doBilling*/
	public class UiType {
		public const int FULLSCREEN = 1;
		public const int COMPACT = 2;
	}
	
	/**
	 * Result of billing action.
	 */
	public class BillingResult
	{
		/** No billing action */
		public const String NONE = "0";
		/** Billing success */
		public const String SUCCESS = "1";
		/** Billing failed, such as sim card is not ready */
		public const String FAILED = "2";
		/** Billing canceled, such as use cancel to purchase it in billing ui.*/
		public const String CANCELLED = "3";
	}
}
