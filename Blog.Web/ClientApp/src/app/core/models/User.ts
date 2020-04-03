/**
 * User model.
 */
export class User {
  /**
   * User Id
   * @param Id string
   * User User Name
   * @param UserName string
   * User Email
   * @param Email string
   * User First Name
   * @param FirstName string
   * User Last Name
   * @param LastName string
   * User Phone Number
   * @param PhoneNumber string
   * User Roles
   * @param Roles Array<string>
   * User Password
   * @param Password string
   * User About
   * @param About string
   */
  constructor(
      public Id: string,
      public UserName: string,

      public Email?: string,
      public FirstName?: string,
      public LastName?: string,
      public PhoneNumber?: string,
      // public ProfileImg?: string,
      public Roles: Array<string> = [],
      public Password?: string,
      public About?: string
  ) { }
}
