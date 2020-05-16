import { PageInfo } from 'src/app/core/models/PageInfo';
import { Post } from 'src/app/core/models/Post';

export class ProfileViewDto {
  /**
   * Profile view dto.
   * @param email string
   * @param firstName string
   * @param lastName string
   * @param phoneNumber string
   * @param password string
   * @param about string
   */
  constructor(
    public email: string,
    public firstName: string,
    public lastName: string,
    public phoneNumber: string,
    public password: string,
    public about: string
  ) {}
}
