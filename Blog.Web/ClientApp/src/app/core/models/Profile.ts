/**
 * Profile model.
 */
export class Profile {
  /**
   * Profile Id
   * @param id string
   * Profile user id
   * @param userId string
   * Profile image
   * @param profileImg string,
   * User about
   * @param about string
   */
  constructor(
      public id: number,
      public userId: string,
      public profileImg?: string,
      public about?: string
  ) { }
}
