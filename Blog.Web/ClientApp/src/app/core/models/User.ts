/**
 * User model.
 */
export class User {
  /**
   * User Id
   * @param id string
   * User user Name
   * @param userName string
   * User email
   * @param mail string
   * User first Name
   * @param firstName string
   * User last Name
   * @param lastName string
   * User phone Number
   * @param phoneNumber string
   * User roles
   * @param roles Array<string>
   * User password
   * @param password string
   * User about
   * @param about string
   */
  constructor(
      public id: string,
      public userName: string,

      public email?: string,
      public firstName?: string,
      public lastName?: string,
      public phoneNumber?: string,
      // public ProfileImg?: string,
      public roles: Array<string> = [],
      public password?: string,
      public about?: string
  ) { }
}
