/** Api routes APiRoutes.*/
export class APiRoutes {
  /**
   * API url.
   * @param API string
   */
  private static readonly API: string = 'api';

  /**
   * VERSION url.
   * @param VERSION string
   */
  private static readonly VERSION: string = 'v1';

  /**
   * BASE url.
   * @param BASE string
   */
  private static readonly BASE: string = APiRoutes.API + '/' + APiRoutes.VERSION;

  // Controllers

  /**
   * Accounts controller url.
   * @param ACCOUNTS_CONTROLLER string
   */
  public static readonly ACCOUNTS_CONTROLLER = APiRoutes.BASE + '/accounts';

  /**
   * Posts controller url.
   * @param POSTS_CONTROLLER string
   */
  public static readonly POSTS_CONTROLLER = APiRoutes.BASE + '/posts';

  /**
   * Comments controller url.
   * @param COMMENTS_CONTROLLER string
   */
  public static readonly COMMENTS_CONTROLLER = APiRoutes.BASE + '/comments';

  /**
   * Comments controller url.
   * @param PROFILE_CONTROLLER string
   */
  public static readonly PROFILE_CONTROLLER = APiRoutes.BASE + '/profile';

  /**
   * Messages controller url.
   * @param MESSAGES_CONTROLLER string
   */
  public static readonly MESSAGES_CONTROLLER = APiRoutes.BASE + '/messages';

  /**
   * Tags controller url.
   * @param TAGS_CONTROLLER string
   */
  public static readonly TAGS_CONTROLLER = APiRoutes.BASE + '/tags';

  // Methods

  // Accounts controller methods
  /**
   * Initialize user method.
   * @param ACCOUNTS_INITIALIZE string
   */
  public static readonly ACCOUNTS_INITIALIZE = APiRoutes.ACCOUNTS_CONTROLLER + '/initialize';

  /**
   * Login user method
   * @param API string
   */
  public static readonly LOGIN_USER = APiRoutes.ACCOUNTS_CONTROLLER + '/login';

  /**
   * Register user method.
   * @param REGISTER_USER string
   */
  public static readonly REGISTER_USER = APiRoutes.ACCOUNTS_CONTROLLER + '/register';

  /**
   * Get all users method.
   * @param GET_ALL_USERS string
   */
  public static readonly GET_ALL_USERS = APiRoutes.ACCOUNTS_CONTROLLER + '/get-all-users';

  /**
   * Change password method.
   * @param CHANGE_PASSWORD string
   */
  public static readonly CHANGE_PASSWORD = APiRoutes.ACCOUNTS_CONTROLLER + '/change-password';

  /**
   * Send confirmation email.
   * @param SEND_CONFIRMATION_EMAIL string
   */
  public static readonly SEND_CONFIRMATION_EMAIL = APiRoutes.ACCOUNTS_CONTROLLER + '/send-confirmation-email';

  /**
   * Users activity method.
   * @param USERS_ACTIVITY string
   */
  public static readonly USERS_ACTIVITY = APiRoutes.ACCOUNTS_CONTROLLER + '/users-activity';

  // Posts controller methods
  /**
   * Create post method.
   * @param CREATE_POST string
   */
  public static readonly CREATE_POST = APiRoutes.POSTS_CONTROLLER + '/create';

  /**
   * Get post method.
   * @param GET_POSTS string
   */
  public static readonly GET_POSTS = APiRoutes.POSTS_CONTROLLER + '/get-posts';

  /**
   * Posts activity method.
   * @param POSTS_ACTIVITY string
   */
  public static readonly POSTS_ACTIVITY = APiRoutes.POSTS_CONTROLLER + '/posts-activity';

  /**
   * Show post method.
   * @param SHOW_POST string
   */
  public static readonly SHOW_POST = APiRoutes.POSTS_CONTROLLER + '/show';

  /**
   * Like post method.
   * @param LIKE_POST string
   */
  public static readonly LIKE_POST = APiRoutes.POSTS_CONTROLLER + '/like';

  /**
   * Dislike post method.
   * @param DISLIKE_POST string
   */
  public static readonly DISLIKE_POST = APiRoutes.POSTS_CONTROLLER + '/dislike';

  /**
   * Users posts method.
   * @param USER_POSTS string
   */
  public static readonly USER_POSTS = APiRoutes.POSTS_CONTROLLER + '/user-posts';

  // Comments controller methods
  /**
   * Get comment by post method.
   * @param GET_COMMENTS_BY_POST string
   */
  public static readonly GET_COMMENTS_BY_POST = APiRoutes.COMMENTS_CONTROLLER + '/get-comments-by-post';

  /**
   * Comments activity method.
   * @param COMMENTS_ACTIVITY string
   */
  public static readonly COMMENTS_ACTIVITY = APiRoutes.COMMENTS_CONTROLLER + '/comments-activity';

  /**
   * Get comments by filter method.
   * @param GET_COMMENTS_BY_FILTER string
   */
  public static readonly GET_COMMENTS_BY_FILTER = APiRoutes.COMMENTS_CONTROLLER + '/get-comments-by-filter';

  /**
   * Create new comment.
   * @param CREATE_COMMENT string
   */
  public static readonly CREATE_COMMENT = APiRoutes.COMMENTS_CONTROLLER + '/create';

  /**
   * Get comment by id.
   * @param GET_COMMENT string
   */
  public static readonly GET_COMMENT = APiRoutes.COMMENTS_CONTROLLER + '/get-comment';

  // Tags controller methods
  /**
   * Get tags.
   * @param GET_TAGS string
   */
  public static readonly GET_TAGS = APiRoutes.TAGS_CONTROLLER + '/get-tags';

  /**
   * Get tags by filter.
   * @param GET_TAGS_BY_FILTER string
   */
  public static readonly GET_TAGS_BY_FILTER = APiRoutes.TAGS_CONTROLLER + '/get-tags-by-filter';

  /**
   * Create new comment.
   * @param CREATE_TAG string
   */
  public static readonly CREATE_TAG = APiRoutes.TAGS_CONTROLLER + '/create';

  /**
   * Get comment by id.
   * @param GET_TAG string
   */
  public static readonly GET_TAG = APiRoutes.TAGS_CONTROLLER + '/get-tag';

  /**
   * Tags activity method.
   * @param TAGS_ACTIVITY string
   */
  public static readonly TAGS_ACTIVITY = APiRoutes.ACCOUNTS_CONTROLLER + '/tags-activity';
}
