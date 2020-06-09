import { Profile } from './Profile';

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
   * @param email string
   * User email confirmed
   * @param emailConfirmed boolean
   * User first Name
   * @param firstName string
   * User last Name
   * @param lastName string
   * User phone Number
   * @param phoneNumber string
   * User phone number confirmed
   * @param phoneNumberConfirmed boolean
   * User roles
   * @param roles Array<string>
   * User password
   * @param password string
   * User profile id
   * @param profileId number
   * User profile
   * @param profile Profile
   */
  constructor(
      public id: string,
      public userName: string,
      public email?: string,
      public emailConfirmed?: boolean,
      public firstName?: string,
      public lastName?: string,
      public phoneNumber?: string,
      public phoneNumberConfirmed?: boolean,
      public roles: Array<string> = [],
      public password?: string,
      public profile?: Profile
  ) { }
}
